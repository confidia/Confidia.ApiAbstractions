namespace TimCodes.ApiAbstractions.Authorization;

public interface IApiUserProvider
{
    Task<IApiUser?> GetUserAsync();
    Task SaveUserAsync(IApiUser user);
}
