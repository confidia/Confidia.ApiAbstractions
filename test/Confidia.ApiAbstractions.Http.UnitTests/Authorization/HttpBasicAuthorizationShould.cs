using Confidia.ApiAbstractions.Http.Authorization.Basic;
using Confidia.ApiAbstractions.Http.Responses;

namespace Confidia.ApiAbstractions.Http.UnitTests.Authorization;

[Collection("http")]
public class HttpBasicAuthorizationShould
{
    private readonly TestFixture _host;
    private readonly DefaultJsonHttpApiClient _client;

    public HttpBasicAuthorizationShould(TestFixture host)
    {
        _host = host;

        _client = new DefaultJsonHttpApiClient(host.Services.GetRequiredService<ILogger<DefaultJsonHttpApiClient>>(), new HttpClient(TestFixture.MockHttp), _host.Services)
        {
            DefaultAuthorizationProvider = _host.Services.GetRequiredService<HttpBasicAuthorizationProvider>()
        };
    }

    [Fact]
    public async Task AddBasicAuth()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net/*")
            .WithHeaders("Authorization", "Basic dGVzdDp0ZXN0")
            .Respond(MediaTypes.Json, "{\"response1\":\"Test\", \"response2\":1}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
    }
}
