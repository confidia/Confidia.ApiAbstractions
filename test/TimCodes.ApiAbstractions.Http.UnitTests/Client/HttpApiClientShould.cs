using System.Net;
using TimCodes.ApiAbstractions.Http.Extensions;
using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.UnitTests.Client;

[Collection("http")]
public class HttpApiClientShould
{
    private readonly TestFixture _host;
    private readonly DefaultJsonHttpApiClient _client;

    public HttpApiClientShould(TestFixture host)
    {
        _host = host;

        _client = new DefaultJsonHttpApiClient(host.Services.GetRequiredService<ILogger<DefaultJsonHttpApiClient>>(), new HttpClient(TestFixture.MockHttp), _host.Services);
    }

    [Fact]
    public async Task ProcessSecondaryResponseDeserializer()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://timcodes.net/*")
            .Respond(HttpStatusCode.NotFound, MediaTypes.Json, "{\"response1\":\"Test\", \"response2\":1}");

        var resolver = _client.GetResolver<TestResponse>();

        resolver.AddVariationForStatusCode(HttpStatusCode.NotFound, _client.GetResponseVariation<TestResponse2>());
        var request = new HttpApiGetRequest(new Uri("http://timcodes.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.False(response.Success);
        var result = response as HttpApiGenericResponse<TestResponse2>;
        Assert.NotNull(result?.Content);
        Assert.Equal("Test", result.Content.Response1);
        Assert.Equal(1, result.Content.Response2);
    }
}
