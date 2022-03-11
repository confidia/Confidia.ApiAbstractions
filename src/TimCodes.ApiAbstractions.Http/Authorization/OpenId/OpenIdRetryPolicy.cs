using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TimCodes.ApiAbstractions.FaultTolerance;

namespace TimCodes.ApiAbstractions.Http.Authorization.OpenId;

public class OpenIdRetryPolicy : IRetryPolicy
{
    private readonly IMemoryCache _cache;
    private readonly HttpOpenIdAuthorizationOptions _options;

    public OpenIdRetryPolicy(IMemoryCache cache, IOptions<HttpOpenIdAuthorizationOptions> options)
    {
        _cache = cache;
        _options = options.Value;
    }

    public Task OnBeforeRetryAsync(IApiRequest request, IApiResponse response, string apiIdentifier)
    {
        if (!_options.TryGetValue(apiIdentifier, out var credentials)) {
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
