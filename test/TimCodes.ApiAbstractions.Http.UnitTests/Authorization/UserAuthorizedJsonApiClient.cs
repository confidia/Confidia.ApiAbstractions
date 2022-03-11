namespace TimCodes.ApiAbstractions.Http.UnitTests.Authorization;

internal class UserAuthorizedJsonApiClient : DefaultJsonHttpApiClient
{
    public UserAuthorizedJsonApiClient(ILogger<UserAuthorizedJsonApiClient> logger, HttpClient httpClient, IServiceProvider serviceProvider) : base(logger, httpClient, serviceProvider)
    {
    }

    public override string ApiIdentifier => "UserAuthorized";
}
