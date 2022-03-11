namespace TimCodes.ApiAbstractions.Authorization;

public interface IAuthorizationProvider
{
    Task AddAuthorizationAsync(IApiRequest request, string apiIdentifier);
}
