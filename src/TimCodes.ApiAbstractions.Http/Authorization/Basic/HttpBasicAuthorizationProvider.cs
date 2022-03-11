using Microsoft.Extensions.Options;
using System.Text;

namespace TimCodes.ApiAbstractions.Http.Authorization.Basic;

/// <summary>
/// Adds a Basic Authorization header to requests using the Username and Password from configuration
/// </summary>
public class HttpBasicAuthorizationProvider : IAuthorizationProvider
{
    private readonly HttpBasicAuthorizationOptions _options;
    private readonly ILogger<HttpBasicAuthorizationProvider> _logger;

    public HttpBasicAuthorizationProvider(IOptions<HttpBasicAuthorizationOptions> options, ILogger<HttpBasicAuthorizationProvider> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public Task AddAuthorizationAsync(IApiRequest request, string apiIdentifier)
    {
        if (request is not HttpApiRequestBase httpRequest)
        {
            throw new InvalidOperationException($"Request is not of type {nameof(HttpApiRequestBase)}");
        }

        if (httpRequest?.Message is null)
        {
            throw new InvalidOperationException("HTTP request has not yet been generated");
        }

        if (!_options.TryGetValue(apiIdentifier, out var credential))
        {
            throw new InvalidOperationException($"No credentials have been configured for {apiIdentifier}");
        }

        var base64 = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{credential.Username}:{credential.Password}"));
        httpRequest.Message.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64);

        _logger.LogDebug("Basic authentication header added to request");

        return Task.CompletedTask;
    }
}
