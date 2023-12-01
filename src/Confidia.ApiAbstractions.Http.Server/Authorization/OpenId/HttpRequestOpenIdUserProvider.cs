using Confidia.ApiAbstractions.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Confidia.ApiAbstractions.Http.Server.Authorization.OpenId;

public class HttpRequestOpenIdUserProvider(IHttpContextAccessor httpContextAccessor, ILogger<HttpRequestOpenIdUserProvider> logger) : IApiUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly ILogger<HttpRequestOpenIdUserProvider> _logger = logger;

    public Task<IApiUser?> GetUserAsync()
    {
        if (_httpContextAccessor.HttpContext is null)
        {
            _logger.LogError("Tried to get Open ID user from non-existant HTTP Context");
            return Task.FromResult<IApiUser?>(null);
        }

        _httpContextAccessor.HttpContext.Items.TryGetValue("OpenIdAccessToken", out var token);
        _httpContextAccessor.HttpContext.Items.TryGetValue("OpenIdRefreshToken", out var refresh);
        _httpContextAccessor.HttpContext.Items.TryGetValue("OpenIdExpiry", out var expiry);

        return Task.FromResult<IApiUser?>(new OpenIdUser
        {
            AccessToken = (string?)token,
            RefreshToken = (string?)refresh,
            Expiry = (DateTimeOffset?)expiry
        });
    }

    public Task SaveUserAsync(IApiUser user)
    {
        if (user is not OpenIdUser openIdUser)
        {
            throw new InvalidOperationException($"User is not {nameof(OpenIdUser)}");
        }

        if (_httpContextAccessor.HttpContext is null)
        {
            _logger.LogError("Tried to set Open ID user on non-existant HTTP Context");
            return Task.CompletedTask;
        }

        _httpContextAccessor.HttpContext.Items["OpenIdAccessToken"] = openIdUser.AccessToken;
        _httpContextAccessor.HttpContext.Items["OpenIdRefreshToken"] = openIdUser.RefreshToken;
        _httpContextAccessor.HttpContext.Items["OpenIdExpiry"] = openIdUser.Expiry;

        return Task.CompletedTask;
    }
}
