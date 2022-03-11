using Microsoft.Extensions.Options;
using System.Text;
using TimCodes.ApiAbstractions.Http.Configuration;
using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.UnitTests.Serialization;

[Collection("http")]
public class ByteArrayHttpApiResponseDeserializerShould
{
    private readonly TestFixture _host;
    private readonly DefaultJsonHttpApiClient _client;

    public ByteArrayHttpApiResponseDeserializerShould(TestFixture host)
    {
        _host = host;

        _client = new DefaultJsonHttpApiClient(
            host.Services.GetRequiredService<ILogger<DefaultJsonHttpApiClient>>(), 
            new HttpClient(TestFixture.MockHttp), 
            _host.Services)
        {
            DefaultResponeDeserlializer = _host.Services.GetRequiredService<ByteArrayHttpApiResponseDeserializer>()
        };
    }

    [Fact]
    public async Task ProcessRequestMessage()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://timcodes.net/*")
            .Respond(MediaTypes.Text, "test");

        var resolver = _client.GetResolver<TestResponse>();
        var request = new HttpApiGetRequest(new Uri("http://timcodes.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        var result = response as HttpApiGenericResponse<byte[]>;
        Assert.NotNull(result?.Content);
        Assert.Equal("test", Encoding.UTF8.GetString(result.Content));
    }
}
