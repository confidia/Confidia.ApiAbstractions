using Confidia.ApiAbstractions.Http.Attributes;

namespace Confidia.ApiAbstractions.Http.Authorization.OpenId;

public class TokenRequest
{
    [HttpFormName("grant_type")]
    public string? GrantType { get; set; }

    [HttpFormName("scope")]
    public string? Scope { get; set; }

    [HttpFormName("client_id")]
    public string? ClientId { get; set; }

    [HttpFormName("client_secret")]
    public string? ClientSecret { get; set; }

    [HttpFormName("refresh_token")]
    public string? RefreshToken { get; set; }
}
