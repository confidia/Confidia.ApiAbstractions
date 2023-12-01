using Confidia.ApiAbstractions.Authorization;
using Confidia.ApiAbstractions.Configuration;
using Confidia.ApiAbstractions.Http.Server.Authorization.OpenId;
using Confidia.ApiAbstractions.Http.Server.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Confidia.ApiAbstractions.Http.Server;
public static class ApiAbstractionsBuilderExtensions
{
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
