using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Security;
using Confidia.ApiAbstractions.Http.Authorization.OpenId;

namespace Confidia.ApiAbstractions.Http.UnitTests.Authorization;

[Collection("http")]
public class HttpOpenIdAuthorizationShould
{
    private readonly TestFixture _host;
    private readonly DefaultJsonHttpApiClient _client;
    private readonly UserAuthorizedJsonApiClient _userClient;

    public HttpOpenIdAuthorizationShould(TestFixture host)
    {
        _host = host;

        _client = new DefaultJsonHttpApiClient(host.Services.GetRequiredService<ILogger<DefaultJsonHttpApiClient>>(), new HttpClient(TestFixture.MockHttp), _host.Services)
        {
            DefaultAuthorizationProvider = _host.Services.GetRequiredService<HttpOpenIdAuthorizationProvider>(),
            DefaultRetryPolicies = new []
            {
                _host.Services.GetRequiredService<OpenIdRetryPolicy>()
            }
        };

        _userClient = new UserAuthorizedJsonApiClient(host.Services.GetRequiredService<ILogger<UserAuthorizedJsonApiClient>>(), new HttpClient(TestFixture.MockHttp), _host.Services)
        {
            DefaultAuthorizationProvider = _host.Services.GetRequiredService<HttpOpenIdAuthorizationProvider>(),
            DefaultRetryPolicies = new[]
            {
                _host.Services.GetRequiredService<OpenIdRetryPolicy>()
            }
        };
    }

    [Fact]
    public async Task AddBearerAuth()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .WithContent("grant_type=client_credentials&scope=openid&client_id=test&client_secret=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test\", \"expires_in\":3600}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net")
            .WithHeaders("Authorization", "Bearer test")
            .Respond(MediaTypes.Json, "{}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);
        _host.Services.GetRequiredService<IMemoryCache>().Remove("openid-token");
    }

    [Fact]
    public async Task AddBearerAuthForUser()
    {
        _host.MockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
        {
            Items = new Dictionary<object, object?>
            {
                { "OpenIdAccessToken", "test" },
                { "OpenIdExpiry", DateTimeOffset.Now.AddSeconds(60) }
            }
        });
        var httpContext = _host.Services.GetRequiredService<IHttpContextAccessor>();

        TestFixture.MockHttp.Clear();

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net")
            .WithHeaders("Authorization", "Bearer test")
            .Respond(MediaTypes.Json, "{}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net"), resolver);

        using var response = await _userClient.SendAsync(request);

        Assert.True(response.Success);
    }

    [Fact]
    public async Task RefreshBearerAuthForUser()
    {
        _host.MockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
        {
            Items = new Dictionary<object, object?>
            {
                { "OpenIdAccessToken", "test" },
                { "OpenIdRefreshToken", "test" },
                { "OpenIdExpiry", DateTimeOffset.Now }
            }
        });
        var httpContext = _host.Services.GetRequiredService<IHttpContextAccessor>();

        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .WithContent("grant_type=refresh_token&scope=openid&client_id=test&client_secret=test&refresh_token=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test2\", \"expires_in\":3600, \"refresh_token\":\"refreshed\"}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net")
            .WithHeaders("Authorization", "Bearer test2")
            .Respond(MediaTypes.Json, "{}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net"), resolver);

        using var response = await _userClient.SendAsync(request);

        Assert.True(response.Success);
        Assert.Equal("test2", httpContext.HttpContext?.Items["OpenIdAccessToken"]);
        Assert.Equal("refreshed", httpContext.HttpContext?.Items["OpenIdRefreshToken"]);
        Assert.True((DateTimeOffset?)httpContext.HttpContext?.Items["OpenIdExpiry"] > DateTimeOffset.Now);
    }

    [Fact]
    public async Task NotRefreshBearerAuthForBadRefreshToken()
    {
        _host.MockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
        {
            Items = new Dictionary<object, object?>
            {
                { "OpenIdAccessToken", "test" },
                { "OpenIdRefreshToken", "test" },
                { "OpenIdExpiry", DateTimeOffset.Now.AddSeconds(10) }
            }
        });
        var httpContext = _host.Services.GetRequiredService<IHttpContextAccessor>();

        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .WithContent("grant_type=refresh_token&scope=openid&client_id=test&client_secret=test&refresh_token=test")
            .Respond(HttpStatusCode.Unauthorized, MediaTypes.Json, "{}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net")
            .WithHeaders("Authorization", "Bearer test2")
            .Respond(MediaTypes.Json, "{}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net"), resolver);
        await Assert.ThrowsAsync<SecurityException>(async () =>
        {
            using var response = await _userClient.SendAsync(request);
        });
    }

    [Fact]
    public async Task CacheToken()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .WithContent("grant_type=client_credentials&scope=openid&client_id=test&client_secret=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test\", \"expires_in\":3600}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net")
            .WithHeaders("Authorization", "Bearer test")
            .Respond(MediaTypes.Json, "{}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net")
            .WithHeaders("Authorization", "Bearer test2")
            .Respond(HttpStatusCode.NotFound, MediaTypes.Json, "{}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);

        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .WithContent("grant_type=client_credentials&scope=openid&client_id=test&client_secret=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test2\", \"expires_in\":3600}");

        using var response2 = await _client.SendAsync(request);

        Assert.True(response2.Success);
        _host.Services.GetRequiredService<IMemoryCache>().Remove("openid-token");
    }

    [Fact]
    public async Task RenewCachedToken()
    {

        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .WithContent("grant_type=client_credentials&scope=openid&client_id=test&client_secret=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test\", \"expires_in\":10}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net/RenewCachedToken")
            .WithHeaders("Authorization", "Bearer test")
            .Respond(MediaTypes.Json, "{}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net/RenewCachedToken"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);

        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .WithContent("grant_type=client_credentials&scope=openid&client_id=test&client_secret=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test2\", \"expires_in\":3600}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net/RenewCachedToken")
            .WithHeaders("Authorization", "Bearer test2")
            .Respond(MediaTypes.Json, "{}");

        using var response2 = await _client.SendAsync(request);

        Assert.True(response2.Success);
        _host.Services.GetRequiredService<IMemoryCache>().Remove("openid-token");
    }

    [Fact]
    public async Task RenewTokenOn401()
    {
        TestFixture.MockHttp.Clear();
        var count = 0;
        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .With(q => ++count == 1)
            .WithContent("grant_type=client_credentials&scope=openid&client_id=test&client_secret=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test\", \"expires_in\":60}");
        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .With(q => ++count >= 2)
            .WithContent("grant_type=client_credentials&scope=openid&client_id=test&client_secret=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test2\", \"expires_in\":60}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net/RenewTokenOn401")
            .WithHeaders("Authorization", "Bearer test")
            .Respond(HttpStatusCode.Unauthorized, MediaTypes.Json, "{}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net/RenewTokenOn401")
            .WithHeaders("Authorization", "Bearer test2")
            .Respond(MediaTypes.Json, "{}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net/RenewTokenOn401"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.True(response.Success);

        _host.Services.GetRequiredService<IMemoryCache>().Remove("openid-token");
    }

    [Fact]
    public async Task NotRenewTokenOnDouble401()
    {
        TestFixture.MockHttp.Clear();

        TestFixture.MockHttp.When(HttpMethod.Post, "http://Confidia.net/token")
            .WithContent("grant_type=client_credentials&scope=openid&client_id=test&client_secret=test")
            .Respond(MediaTypes.Json, "{\"access_token\":\"test\", \"expires_in\":3600}");

        TestFixture.MockHttp.When(HttpMethod.Get, "http://Confidia.net/RenewTokenOn401")
            .WithHeaders("Authorization", "Bearer test")
            .Respond(HttpStatusCode.Unauthorized, MediaTypes.Json, "{}");

        var resolver = _client.GetResolver<TestResponse>();

        var request = new HttpApiGetRequest(new Uri("http://Confidia.net/RenewTokenOn401"), resolver);

        using var response = await _client.SendAsync(request);

        Assert.False(response.Success);

        _host.Services.GetRequiredService<IMemoryCache>().Remove("openid-token");
    }
}
