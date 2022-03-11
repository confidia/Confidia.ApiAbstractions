using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.UnitTests.Serialization;

[Collection("http")]
public class JsonHttpApiResponseDeserializerShould
{
    private readonly TestFixture _host;
    private readonly DefaultJsonHttpApiClient _client;

    public JsonHttpApiResponseDeserializerShould(TestFixture host)
    {
        _host = host;

        _client = new DefaultJsonHttpApiClient(host.Services.GetRequiredService<ILogger<DefaultJsonHttpApiClient>>(), new HttpClient(TestFixture.MockHttp), _host.Services);
    }

    [Fact]
    public async Task ProcessRequestMessage()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://timcodes.net/*")
            .Respond(MediaTypes.Json, "{\"response1\":\"Test\", \"response2\":1}");

        var resolver = _client.GetResolver<TestResponse>();
        var request = new HttpApiGetRequest(new Uri("http://timcodes.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        var result = response as HttpApiGenericResponse<TestResponse>;
        Assert.NotNull(result?.Content);
        Assert.Equal("Test", result.Content.Response1);
        Assert.Equal(1, result.Content.Response2);
    }
}
