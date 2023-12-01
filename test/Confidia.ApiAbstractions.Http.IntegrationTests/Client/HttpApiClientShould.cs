using Confidia.ApiAbstractions.Http.Responses;
using Confidia.ApiAbstractions.Http.TestModels.Reqres;
using Confidia.ApiAbstractions.Serialization;

namespace Confidia.ApiAbstractions.Http.IntegrationTests.Client;

[Collection("http")]
public class HttpApiClientShould
{
    private readonly TestFixture _host;
    private readonly DefaultJsonHttpApiClient _client;

    public HttpApiClientShould(TestFixture host)
    {
        _host = host;

        _client = _host.Services.GetRequiredService<DefaultJsonHttpApiClient>();
    }

    [Fact]
    public async Task SendJsonGetRequest()
    {
        var resolver = _client.GetResolver<User>();

        var request = new HttpApiGetRequest(new Uri("https://reqres.in/api/users/2"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        var result = response as HttpApiGenericResponse<User>;
        Assert.NotNull(result?.Content);
        Assert.Equal(2, result.Content.Data.Id);
        Assert.Equal("Janet", result.Content.Data.FirstName);
    }

    [Fact]
    public async Task SendJsonPostRequest()
    {
        var resolver = _client.GetResolver<CreatedUser>();

        var request = new HttpApiPostRequest(new Uri("https://reqres.in/api/users/"), new
        {
            name = "morpheus",
            job = "leader"
        }, resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        var result = response as HttpApiGenericResponse<CreatedUser>;
        Assert.NotNull(result?.Content);
        Assert.Equal("morpheus", result.Content.Name);
    }

    [Fact]
    public async Task SendJsonPutRequest()
    {
        var resolver = _client.GetResolver<CreatedUser>();

        var request = new HttpApiPutRequest(new Uri("https://reqres.in/api/users/"), new
        {
            name = "morpheus",
            job = "leader"
        }, resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        var result = response as HttpApiGenericResponse<CreatedUser>;
        Assert.NotNull(result?.Content);
        Assert.Equal("morpheus", result.Content.Name);
    }

    [Fact]
    public async Task SendJsonPatchRequest()
    {
        var resolver = _client.GetResolver<CreatedUser>();

        var request = new HttpApiPatchRequest(new Uri("https://reqres.in/api/users/"), new
        {
            name = "morpheus",
            job = "leader"
        }, resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        var result = response as HttpApiGenericResponse<CreatedUser>;
        Assert.NotNull(result?.Content);
        Assert.Equal("morpheus", result.Content.Name);
    }

    [Fact]
    public async Task SendJsonDeleteRequest()
    {
        var resolver = _client.GetUntypedResolver(_host.Services.GetRequiredService<EmptyHttpApiResponseDeserializer>());

        var request = new HttpApiDeleteRequest(new Uri("https://reqres.in/api/users/2"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        var result = response as HttpApiEmptyResponse;
        Assert.NotNull(result);
    }
}
