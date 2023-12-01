using Microsoft.Extensions.Options;
using Confidia.ApiAbstractions.Http.Configuration;

namespace Confidia.ApiAbstractions.Http;

public class DefaultJsonHttpApiClient : HttpApiClientBase
{
    public DefaultJsonHttpApiClient(ILogger<DefaultJsonHttpApiClient> logger, HttpClient httpClient, IServiceProvider serviceProvider) : 
        base(logger, httpClient, serviceProvider)
    {
        DefaultRequestSerlializer = serviceProvider.GetRequiredService<JsonHttpApiRequestSerializer>();
        DefaultResponeDeserlializer = serviceProvider.GetRequiredService<JsonHttpApiResponseDeserializer>();
    }
}
