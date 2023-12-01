using System.Net;
using System.Text.Json;
using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.UnitTests.Serialization;

[Collection("http")]
public class DotNetBadRequestHttpApiResponseDeserializer
{
    private readonly TestFixture _host;
    private readonly DefaultJsonHttpApiClient _client;

    public DotNetBadRequestHttpApiResponseDeserializer(TestFixture host)
    {
        _host = host;

        _client = new DefaultJsonHttpApiClient(
            host.Services.GetRequiredService<ILogger<DefaultJsonHttpApiClient>>(), 
            new HttpClient(TestFixture.MockHttp), 
            _host.Services)
        {
            DefaultResponeDeserlializer = _host.Services.GetRequiredService<JsonHttpApiResponseDeserializer>()
        };
    }

    [Fact]
    public async Task ProcessSuccessfulRequestMessage()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://timcodes.net/*")
            .Respond(MediaTypes.Json, "{\"response1\":\"Test\", \"response2\":1}");

        var resolver = _client.GetResolverWithDotNetBadRequestHandling<TestResponse>();
        var request = new HttpApiGetRequest(new Uri("http://timcodes.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        var result = response as HttpApiGenericResponse<TestResponse>;
        Assert.NotNull(result?.Content);
        Assert.Equal("Test", result.Content.Response1);
        Assert.Equal(1, result.Content.Response2);
    }

    [Fact]
    public async Task ProcessStandardBadRequestMessage()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://timcodes.net/*")
            .Respond(HttpStatusCode.BadRequest, MediaTypes.Json, "{\"response1\":\"Test\", \"response2\":1}");

        var resolver = _client.GetResolverWithDotNetBadRequestHandling<TestResponse>();
        var request = new HttpApiGetRequest(new Uri("http://timcodes.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.False(response.Success);
        var result = response as HttpApiGenericResponse<TestResponse>;
        Assert.NotNull(result?.Content);
        Assert.Equal("Test", result.Content.Response1);
        Assert.Equal(1, result.Content.Response2);
    }

    [Fact]
    public async Task ProcessDotNetBadRequestMessage()
    {
        var badRequest = new DotNetValidationProblem
        {
            Detail = "detail",
            Title = "title",
            Errors = new Dictionary<string, string[]>
            {
                { "test", new[] {"test"} }
            },
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = 0
        };

        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://timcodes.net/*")
            .Respond(HttpStatusCode.BadRequest, MediaTypes.Json, JsonSerializer.Serialize(badRequest, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));

        var resolver = _client.GetResolverWithDotNetBadRequestHandling<TestResponse>();
        var request = new HttpApiGetRequest(new Uri("http://timcodes.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.False(response.Success);
        var result = response as HttpApiValidationResponse;
        Assert.NotNull(result?.Content);
        Assert.Equal("title", result.Content.Title);
        Assert.Equal("detail", result.Content.Detail);
        Assert.Equal("test", result.Content?.Errors is null ? null : result.Content?.Errors["test"].First());
        Assert.Equal(0, result.Content?.Status);
    }
}
