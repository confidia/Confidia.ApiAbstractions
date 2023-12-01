using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Moq;
using Confidia.ApiAbstractions.Http.Authorization.OpenId;

namespace Confidia.ApiAbstractions.Http.UnitTests;

[CollectionDefinition("http")]
public class HttpCollection : ICollectionFixture<TestFixture>
{

}

public class TestFixture : IAsyncDisposable, IDisposable
{
    public TestFixture()
    {
        MockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        IHost host = Setup(MockHttpContextAccessor.Object);
        Configuration = host.Services.GetRequiredService<IConfiguration>();
        Logger = host.Services.GetRequiredService<ILogger<TestFixture>>();
        Services = host.Services;
    }

    public static readonly MockHttpMessageHandler MockHttp = new ();
    public IConfiguration Configuration { get; }
    public IServiceProvider Services { get; }
    public ILogger Logger { get; }
    public Mock<IHttpContextAccessor> MockHttpContextAccessor { get; }

    internal static IHost Setup(IHttpContextAccessor httpContextAccessor) =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();
                services
                    .AddApiAbstractions(context.Configuration)
                    .AddHttp()
                    .AddBasicAuthorization()
                    .AddOpenIdAuthorization();
                services.AddSingleton(httpContextAccessor);
                services.AddHttpClient<OpenIdClient>().ConfigurePrimaryHttpMessageHandler(q => MockHttp);
            })
            .Build();

    public void Dispose() => DisposeAsync().AsTask().Wait();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
