using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Security;

namespace Confidia.ApiAbstractions.Http.Authorization.OpenId;

/// <summary>
/// Adds a Bearer Authorization header to requests using the OpenID options from configuration
/// </summary>
public class HttpOpenIdAuthorizationProvider(
    IOptions<HttpOpenIdAuthorizationOptions> options,
    ILogger<HttpOpenIdAuthorizationProvider> logger,
    IMemoryCache cache,
    OpenIdClient client,
    IServiceProvider serviceProvider) : IAuthorizationProvider
{
    private readonly HttpOpenIdAuthorizationOptions _options = options.Value;
    private readonly ILogger<HttpOpenIdAuthorizationProvider> _logger = logger;
    private readonly IMemoryCache _cache = cache;
    private readonly OpenIdClient _client = client;
    private readonly IApiUserProvider? _apiUserProvider = serviceProvider.GetService<IApiUserProvider>();

    public async Task AddAuthorizationAsync(IApiRequest request, string apiIdentifier)
    {
        if (request is not HttpApiRequestBase httpRequest)
        {
            throw new InvalidOperationException($"Request is not of type {nameof(HttpApiRequestBase)}");
        }

        if (httpRequest?.Message is null)
        {
            throw new InvalidOperationException("HTTP request has not yet been generated");
        }

        if (!_options.TryGetValue(apiIdentifier, out var credential))
        {
            throw new InvalidOperationException($"No credentials have been configured for {apiIdentifier}");
        }

        switch(credential.Flow)
        {
            case OpenIdFlowType.ClientCredentials:
                TokenResponse? accessToken = await _cache.GetOrCreateAsync(credential.TokenCacheKey, async entry =>
                {
                    var token = await _client.GetClientCredentialsTokenAsync(credential);
                    var remainingTime = token.ExpiresIn - credential.MinTokenLifetime.TotalSeconds;

                    if (remainingTime > 0)
                    {
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(remainingTime);
                    }
                    else
                    {
                        entry.AbsoluteExpiration = DateTimeOffset.Now.AddMilliseconds(-1);
                    }

                    return token;
                }) 
                    ?? throw new SecurityException("Unable to get an access token for the requested resource");

                httpRequest.Message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);

                _logger.LogDebug("Bearer authentication header added to request");

                break;

            case OpenIdFlowType.User:
                if (_apiUserProvider is null)
                {
                    throw new InvalidOperationException($"No {nameof(IApiUserProvider)} registered");
                }

                var user = await _apiUserProvider.GetUserAsync();
                if (user is null)
                {
                    _logger.LogWarning("No user found by {apiProvider}", _apiUserProvider.GetType().Name);
                    return;
                }

                if (user is not OpenIdUser openIdUser)
                {
                    _logger.LogWarning("User returned by {apiProvider} was not an Open ID user", _apiUserProvider.GetType().Name);
                    return;
                }

                if (openIdUser is null)
                {
                    throw new SecurityException("Unable to get an access token for the current user for the requested resource");
                }

                //Check expiry and refresh if necessary
                if (openIdUser.Expiry <= DateTimeOffset.Now + credential.MinTokenLifetime && !string.IsNullOrEmpty(openIdUser.RefreshToken))
                {
                    var token = await _client.RefreshAccessTokenAsync(credential, openIdUser);

                    if (token?.AccessToken is null)
                    {
                        throw new SecurityException("Unable to get an access token for the current user for the requested resource, even after using a refresh token");
                    }

                    if (!string.IsNullOrEmpty(token.RefreshToken)) openIdUser.RefreshToken = token.RefreshToken;
                    openIdUser.AccessToken = token.AccessToken;
                    openIdUser.Expiry = DateTimeOffset.Now.AddSeconds(token.ExpiresIn);

                    await _apiUserProvider.SaveUserAsync(openIdUser);
                }

                httpRequest.Message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", openIdUser.AccessToken);

                _logger.LogDebug("Bearer authentication header added to request");

                break;
        }
    }
}
