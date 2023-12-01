using Microsoft.Extensions.Configuration;

namespace Confidia.ApiAbstractions.Configuration;

public class ApiAbstractionsBuilder(IServiceCollection services, IConfiguration config)
{
    public IServiceCollection Services { get; } = services;

    public IConfiguration Configuration { get; } = config;

    public const string MainConfigSection = "ApiOptions";
}
