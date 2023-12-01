using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Confidia.ApiAbstractions.Http.IntegrationTests;

[CollectionDefinition("http")]
public class HttpCollection : ICollectionFixture<TestFixture>
{

}

public class TestFixture : IAsyncDisposable, IDisposable
{
    public TestFixture()
    {
        IHost host = Setup();
        Configuration = host.Services.GetRequiredService<IConfiguration>();
        Logger = host.Services.GetRequiredService<ILogger<TestFixture>>();
        Services = host.Services;
    }

    public IConfiguration Configuration { get; }
    public IServiceProvider Services { get; }
    public ILogger Logger { get; }

    internal static IHost Setup() =>
        Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddLogging();
                services.AddApiAbstractions(context.Configuration)
                    .AddHttp();
            })
            .Build();

    public void Dispose() => DisposeAsync().AsTask().Wait();

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
