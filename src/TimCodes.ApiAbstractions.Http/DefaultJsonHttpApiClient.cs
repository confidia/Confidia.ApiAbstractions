using Microsoft.Extensions.Options;
using TimCodes.ApiAbstractions.Http.Configuration;

namespace TimCodes.ApiAbstractions.Http;

public class DefaultJsonHttpApiClient : HttpApiClientBase
{
    public DefaultJsonHttpApiClient(ILogger<DefaultJsonHttpApiClient> logger, HttpClient httpClient, IServiceProvider serviceProvider) : 
        base(logger, httpClient, serviceProvider)
    {
        DefaultRequestSerlializer = serviceProvider.GetRequiredService<JsonHttpApiRequestSerializer>();
        DefaultResponeDeserlializer = serviceProvider.GetRequiredService<JsonHttpApiResponseDeserializer>();
    }
}
