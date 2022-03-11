using Microsoft.Extensions.Configuration;
using TimCodes.ApiAbstractions.Configuration;

namespace TimCodes.ApiAbstractions;

public static class ServiceCollectionExtensions
{
    public static ApiAbstractionsBuilder AddApiAbstractions(this IServiceCollection services, IConfiguration config)
    {
        var builder = new ApiAbstractionsBuilder(services, config);

        //Serializers
        builder.Services.AddSingleton<EmptyApiRequestSerializer>();

        //Deserializers
        builder.Services.AddSingleton<EmptyApiResponseDeserializer>();

        return builder;
    }
}
