namespace Confidia.ApiAbstractions.Http.Authorization.OpenId;

public class HttpOpenIdAuthorizationCredentials
{
    public Uri? TokenUri { get; set; }

    public string Scope { get; set; } = "openid";

    public OpenIdFlowType Flow { get; set; } = OpenIdFlowType.ClientCredentials;

    public string ClientId { get; set; } = string.Empty;

    public string? ClientSecret { get; set; } = string.Empty;

    public string TokenCacheKey { get; set; } = "openid-token";

    public TimeSpan MinTokenLifetime { get; set; } = TimeSpan.FromSeconds(30);
}

public class HttpOpenIdAuthorizationOptions : Dictionary<string, HttpOpenIdAuthorizationCredentials>
{
    
}
