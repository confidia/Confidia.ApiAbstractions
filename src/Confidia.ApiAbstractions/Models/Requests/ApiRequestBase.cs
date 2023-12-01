using Confidia.ApiAbstractions.FaultTolerance;

namespace Confidia.ApiAbstractions.Models.Requests;

/// <summary>
/// A basis for all API requests
/// </summary>
public abstract class ApiRequestBase : IApiRequest
{
    public abstract bool HasMessage { get; }

    public IApiRequestSerializer? Serializer { get; set; }

    public IAuthorizationProvider? AuthorizationProvider { get; set; }

    public ApiResponseVariationResolver? Resolver { get; set; }

    public Action? OnBeforeSend { get; set; }

    public IRetryPolicy[] RetryPolicies { get; set; } = Array.Empty<IRetryPolicy>();
}
