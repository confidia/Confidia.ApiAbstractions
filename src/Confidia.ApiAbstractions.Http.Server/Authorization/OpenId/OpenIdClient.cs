using System.Security;
using Confidia.ApiAbstractions.Http.Responses;
using Confidia.ApiAbstractions.Http.Serialization;
using Confidia.ApiAbstractions.Http.Server.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Confidia.ApiAbstractions.Http.Server.Authorization.OpenId;

public class OpenIdClient : HttpApiClientBase
{
    public OpenIdClient(ILogger<OpenIdClient> logger, HttpClient httpClient, IServiceProvider serviceProvider) : base(logger, httpClient, serviceProvider)
    {
        DefaultRequestSerlializer = serviceProvider.GetRequiredService<FormHttpApiRequestSerializer>();
        DefaultResponeDeserlializer = serviceProvider.GetRequiredService<JsonHttpApiResponseDeserializer>();
    }

    public override string ApiIdentifier => "__OpenId";

    public async Task<TokenResponse> GetClientCredentialsTokenAsync(HttpOpenIdAuthorizationCredentials credentials)
    {
        if (credentials.TokenUri is null)
        {
            throw new ArgumentNullException(nameof(credentials.TokenUri), "Token URI not configured");
        }

        if (string.IsNullOrEmpty(credentials.ClientId))
        {
            throw new ArgumentNullException(nameof(credentials.ClientId), "Client ID not configured");
        }

        if (string.IsNullOrEmpty(credentials.ClientSecret))
        {
            throw new ArgumentNullException(nameof(credentials.ClientSecret), "Client secret not configured");
        }

        var tokenRequest = new TokenRequest
        {
            ClientId = credentials.ClientId,
            ClientSecret = credentials.ClientSecret,
            GrantType = "client_credentials",
            Scope = credentials.Scope
        };

        var request = CreatePostRequest<TokenResponse>(credentials.TokenUri.AbsoluteUri, tokenRequest);
        var response = await SendAsync(request);
        if (response.Success)
        {
            if (response is HttpApiGenericResponse<TokenResponse> populatedResponse && populatedResponse.Content is not null)
            {
                return populatedResponse.Content;
            }
        }

        var httpResponse = (HttpApiResponseBase)response;
        var rawResponse = await httpResponse.RawMessage.Content.ReadAsStringAsync();

        throw new SecurityException($"Unable to retrieve access token{Environment.NewLine}\t-Status code: {httpResponse.StatusCode}\t-Content: {rawResponse}");
    }

    public async Task<TokenResponse> RefreshAccessTokenAsync(HttpOpenIdAuthorizationCredentials credentials, OpenIdUser user)
    {
        if (credentials.TokenUri is null)
        {
            throw new ArgumentNullException(nameof(credentials.TokenUri), "Token URI not configured");
        }

        if (string.IsNullOrEmpty(credentials.ClientId))
        {
            throw new ArgumentNullException(nameof(credentials.ClientId), "Client ID not configured");
        }

        if (string.IsNullOrEmpty(user.RefreshToken))
        {
            throw new ArgumentNullException(nameof(user.RefreshToken), "Refresh token missing on user");
        }

        var tokenRequest = new TokenRequest
        {
            ClientId = credentials.ClientId,
            ClientSecret = credentials.ClientSecret,
            GrantType = "refresh_token",
            Scope = credentials.Scope,
            RefreshToken = user.RefreshToken
        };

        var request = CreatePostRequest<TokenResponse>(credentials.TokenUri.AbsoluteUri, tokenRequest);
        var response = await SendAsync(request);
        if (response.Success)
        {
            if (response is HttpApiGenericResponse<TokenResponse> populatedResponse && populatedResponse.Content is not null)
            {
                return populatedResponse.Content;
            }
        }

        var httpResponse = (HttpApiResponseBase)response;
        var rawResponse = await httpResponse.RawMessage.Content.ReadAsStringAsync();

        throw new SecurityException($"Unable to retrieve access token{Environment.NewLine}\t-Status code: {httpResponse.StatusCode}\t-Content: {rawResponse}");
    }
}
