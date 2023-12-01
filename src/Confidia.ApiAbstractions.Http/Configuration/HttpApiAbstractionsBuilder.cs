using Microsoft.Extensions.Configuration;

namespace Confidia.ApiAbstractions.Configuration;

public class HttpApiAbstractionsBuilder : ApiAbstractionsBuilder
{
    public HttpApiAbstractionsBuilder(ApiAbstractionsBuilder builder) : base(builder.Services, builder.Configuration)
    {
    }

    public HttpApiAbstractionsBuilder(IServiceCollection services, IConfiguration config) : base(services, config)
    {
    }

    public const string ConfigSection = "Http";
}
