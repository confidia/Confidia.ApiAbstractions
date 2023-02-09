using System.Net;
using TimCodes.ApiAbstractions.Http.Extensions;
using TimCodes.ApiAbstractions.Http.Responses;
using TimCodes.ApiAbstractions.Models.Responses;

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

    [Fact]
    public async Task ProcessException()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://timcodes.net/*")
            .Respond(HttpStatusCode.NotFound, MediaTypes.Text, "<>");

        var resolver = _client.GetResolver<TestResponse>();
        var request = new HttpApiGetRequest(new Uri("http://timcodes.net"), resolver);
        var counter = 0;
        using var response = await _client.SendAsync<TestResponse, ErrorApiResponse>(request, success =>
        {
            counter = 1;
            return Task.CompletedTask;
        }, 
        failure =>
        {
            counter = 2;
            return Task.CompletedTask;
        },
        exception =>
        {
            counter = 3;
            return Task.CompletedTask;
        });

        Assert.Equal(3, counter);
    }

    [Fact]
    public async Task ProcessSuccess()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://timcodes.net/*")
            .Respond(HttpStatusCode.Accepted, MediaTypes.Json, "{\"response1\":\"Test\", \"response2\":1}");

        var resolver = _client.GetResolver<TestResponse>();
        var request = new HttpApiGetRequest(new Uri("http://timcodes.net"), resolver);
        var counter = 0;
        using var response = await _client.SendAsync<HttpApiGenericResponse<TestResponse>, ErrorApiResponse>(request, success =>
        {
            counter = 1;
            return Task.CompletedTask;
        },
        failure =>
        {
            counter = 2;
            return Task.CompletedTask;
        });

        Assert.Equal(1, counter);
    }
}
