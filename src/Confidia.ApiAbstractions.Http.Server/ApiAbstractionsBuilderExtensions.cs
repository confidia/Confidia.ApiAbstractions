using Confidia.ApiAbstractions.Authorization;
using Confidia.ApiAbstractions.Configuration;
using Confidia.ApiAbstractions.Http.Server.Authorization.OpenId;
using Confidia.ApiAbstractions.Http.Server.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Confidia.ApiAbstractions.Http.Server;
public static class ApiAbstractionsBuilderExtensions
{
    public static HttpApiAbstractionsBuilder AddOpenIdAuthorization(this HttpApiAbstractionsBuilder builder, ServiceLifetime openIdLifetime = ServiceLifetime.Scoped)
    {
        builder.Services.Configure<HttpOpenIdAuthorizationOptions>(
            builder.Configuration.GetSection($"{ApiAbstractionsBuilder.MainConfigSection}:{HttpApiAbstractionsBuilder.ConfigSection}:OpenIdAuthorization"));

        builder.Services.AddMemoryCache();
        builder.Services.Add(new ServiceDescriptor(typeof(HttpOpenIdAuthorizationProvider), typeof(HttpOpenIdAuthorizationProvider), openIdLifetime));
        builder.Services.Add(new ServiceDescriptor(typeof(IApiUserProvider), typeof(HttpRequestOpenIdUserProvider), openIdLifetime));

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<OpenIdRetryPolicy>();

        return builder;
    }
}
