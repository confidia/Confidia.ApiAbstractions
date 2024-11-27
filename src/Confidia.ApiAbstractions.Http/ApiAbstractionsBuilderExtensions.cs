using Confidia.ApiAbstractions.Configuration;
using Confidia.ApiAbstractions.Http.Authorization.Basic;
using Confidia.ApiAbstractions.Http.Configuration;

namespace Confidia.ApiAbstractions.Http;

public static class ApiAbstractionsBuilderExtensions
{
    public static HttpApiAbstractionsBuilder AddHttp(this ApiAbstractionsBuilder builder, ServiceLifetime clientLifetime = ServiceLifetime.Scoped)
    {
        var httpBuilder = new HttpApiAbstractionsBuilder(builder);

        httpBuilder.Services.Configure<HttpApiCollectionOptions>(
            httpBuilder.Configuration.GetSection($"{ApiAbstractionsBuilder.MainConfigSection}:{HttpApiAbstractionsBuilder.ConfigSection}"));

        //Serializers
        httpBuilder.Services.AddSingleton<FormHttpApiRequestSerializer>();
        httpBuilder.Services.AddSingleton<JsonHttpApiRequestSerializer>();
        httpBuilder.Services.AddSingleton<TextHttpApiRequestSerializer>();

        //Deserializers
        httpBuilder.Services.AddSingleton<ByteArrayHttpApiResponseDeserializer>();
        httpBuilder.Services.AddSingleton<JsonHttpApiResponseDeserializer>();
        httpBuilder.Services.AddSingleton<StreamHttpApiResponseDeserializer>();
        httpBuilder.Services.AddSingleton<TextHttpApiResponseDeserializer>();
        httpBuilder.Services.AddSingleton<EmptyHttpApiResponseDeserializer>();
        httpBuilder.Services.AddSingleton<DotNetBadRequestHttpApiResponseDeserializer>();

        //Default
        httpBuilder.Services.Add(new ServiceDescriptor(typeof(DefaultJsonHttpApiClient), typeof(DefaultJsonHttpApiClient), clientLifetime));

        httpBuilder.Services.AddHttpClient();

        return httpBuilder;
    }

    public static HttpApiAbstractionsBuilder AddBasicAuthorization(this HttpApiAbstractionsBuilder builder, ServiceLifetime providerLifetime = ServiceLifetime.Scoped)
    {
        builder.Services.Configure<HttpBasicAuthorizationOptions>(
            builder.Configuration.GetSection($"{ApiAbstractionsBuilder.MainConfigSection}:{HttpApiAbstractionsBuilder.ConfigSection}:BasicAuthorization"));

        
        builder.Services.Add(new ServiceDescriptor(typeof(HttpBasicAuthorizationProvider), typeof(HttpBasicAuthorizationProvider), providerLifetime));

        return builder;
    }

    
}
