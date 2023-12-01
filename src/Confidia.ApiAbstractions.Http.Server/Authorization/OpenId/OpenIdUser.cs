using Confidia.ApiAbstractions.Authorization;

namespace Confidia.ApiAbstractions.Http.Server.Authorization.OpenId;

public class OpenIdUser : IApiUser
{
    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateTimeOffset? Expiry { get; set; }
}
