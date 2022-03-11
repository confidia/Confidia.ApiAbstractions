using Microsoft.Extensions.Configuration;

namespace TimCodes.ApiAbstractions.Configuration;

public class ApiAbstractionsBuilder
{
    public ApiAbstractionsBuilder(IServiceCollection services, IConfiguration config)
    {
        Services = services;
        Configuration = config;
    }

    public IServiceCollection Services { get; }

    public IConfiguration Configuration { get; }

    public const string MainConfigSection = "ApiOptions";
}
