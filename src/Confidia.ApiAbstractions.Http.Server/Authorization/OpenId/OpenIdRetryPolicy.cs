using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Confidia.ApiAbstractions.FaultTolerance;
using Confidia.ApiAbstractions.Models.Requests;
using Confidia.ApiAbstractions.Models.Responses;
using Confidia.ApiAbstractions.Http.Responses;
using System.Net;
using Confidia.ApiAbstractions.Http.Server.Configuration;

namespace Confidia.ApiAbstractions.Http.Server.Authorization.OpenId;

public class OpenIdRetryPolicy(IMemoryCache cache, IOptions<HttpOpenIdAuthorizationOptions> options) : IRetryPolicy
{
    private readonly IMemoryCache _cache = cache;
    private readonly HttpOpenIdAuthorizationOptions _options = options.Value;

    public Task OnBeforeRetryAsync(IApiRequest request, IApiResponse response, string apiIdentifier)
    {
        if (!_options.TryGetValue(apiIdentifier, out var credentials))
        {
            throw new InvalidOperationException($"Configuration not provided for {apiIdentifier}");
        }

        _cache.Remove(credentials.TokenCacheKey);
        return Task.CompletedTask;
    }

    public bool ShouldRetry(IApiResponse response, int attempt) =>
        response is HttpApiResponseBase genericResponse ?
            genericResponse.StatusCode == HttpStatusCode.Unauthorized && attempt == 1 :
            false;
}
