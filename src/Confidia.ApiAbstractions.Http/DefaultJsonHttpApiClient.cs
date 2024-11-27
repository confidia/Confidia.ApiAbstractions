namespace Confidia.ApiAbstractions.Http;

public class DefaultJsonHttpApiClient : HttpApiClientBase
{
    public DefaultJsonHttpApiClient(ILogger<DefaultJsonHttpApiClient> logger, HttpClient httpClient, IServiceProvider serviceProvider) : 
        base(logger, httpClient, serviceProvider)
    {
    }

    public DefaultJsonHttpApiClient(ILogger<DefaultJsonHttpApiClient> logger, IHttpClientFactory httpClientFactory, string httpClientName, IServiceProvider serviceProvider) : base(logger, httpClientFactory, httpClientName, serviceProvider)
    {
    }

    protected override void SetupClient()
    {
        base.SetupClient();

        DefaultRequestSerlializer = ServiceProvider.GetRequiredService<JsonHttpApiRequestSerializer>();
        DefaultResponeDeserlializer = ServiceProvider.GetRequiredService<JsonHttpApiResponseDeserializer>();
    }
}
