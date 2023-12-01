using Microsoft.AspNetCore.Http;
using Confidia.ApiAbstractions.Configuration;
using Confidia.ApiAbstractions.Http.Authorization.Basic;
using Confidia.ApiAbstractions.Http.Authorization.OpenId;
using Confidia.ApiAbstractions.Http.Configuration;

namespace Confidia.ApiAbstractions.Http;

public static class ApiAbstractionsBuilderExtensions
{
    public static HttpApiAbstractionsBuilder AddHttp(this ApiAbstractionsBuilder builder)
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
        httpBuilder.Services.AddScoped<DefaultJsonHttpApiClient>();

        httpBuilder.Services.AddHttpClient();

        return httpBuilder;
    }

    public static HttpApiAbstractionsBuilder AddBasicAuthorization(this HttpApiAbstractionsBuilder builder)
    {
        builder.Services.Configure<HttpBasicAuthorizationOptions>(
            builder.Configuration.GetSection($"{ApiAbstractionsBuilder.MainConfigSection}:{HttpApiAbstractionsBuilder.ConfigSection}:BasicAuthorization"));

        builder.Services.AddScoped<HttpBasicAuthorizationProvider>();

        return builder;
    }

    public static HttpApiAbstractionsBuilder AddOpenIdAuthorization(this HttpApiAbstractionsBuilder builder)
    {
        builder.Services.Configure<HttpOpenIdAuthorizationOptions>(
            builder.Configuration.GetSection($"{ApiAbstractionsBuilder.MainConfigSection}:{HttpApiAbstractionsBuilder.ConfigSection}:OpenIdAuthorization"));

        builder.Services.AddMemoryCache();
        builder.Services.AddScoped<HttpOpenIdAuthorizationProvider>();
        builder.Services.AddScoped<IApiUserProvider, HttpRequestOpenIdUserProvider>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<OpenIdRetryPolicy>();

        return builder;
    }
}
