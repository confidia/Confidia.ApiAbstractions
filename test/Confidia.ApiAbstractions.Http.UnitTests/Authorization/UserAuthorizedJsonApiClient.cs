namespace Confidia.ApiAbstractions.Http.UnitTests.Authorization;

internal class UserAuthorizedJsonApiClient(ILogger<UserAuthorizedJsonApiClient> logger, HttpClient httpClient, IServiceProvider serviceProvider) : DefaultJsonHttpApiClient(logger, httpClient, serviceProvider)
{
    public override string ApiIdentifier => "UserAuthorized";
}
