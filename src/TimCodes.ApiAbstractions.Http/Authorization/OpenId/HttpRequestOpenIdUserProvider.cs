using Microsoft.AspNetCore.Http;

namespace TimCodes.ApiAbstractions.Http.Authorization.OpenId;

public class HttpRequestOpenIdUserProvider : IApiUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpRequestOpenIdUserProvider(IHttpContextAccessor httpContextAccessor) 
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<IApiUser> GetUserAsync()
    {
        _httpContextAccessor.HttpContext.Items.TryGetValue("OpenIdAccessToken", out var token);
        _httpContextAccessor.HttpContext.Items.TryGetValue("OpenIdRefreshToken", out var refresh);
        _httpContextAccessor.HttpContext.Items.TryGetValue("OpenIdExpiry", out var expiry);

        return Task.FromResult<IApiUser>(new OpenIdUser
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

        _httpContextAccessor.HttpContext.Items["OpenIdAccessToken"] = openIdUser.AccessToken;
        _httpContextAccessor.HttpContext.Items["OpenIdRefreshToken"] = openIdUser.RefreshToken;
        _httpContextAccessor.HttpContext.Items["OpenIdExpiry"] = openIdUser.Expiry;

        return Task.CompletedTask;
    }
}
