using Confidia.ApiAbstractions.FaultTolerance;

namespace Confidia.ApiAbstractions.Models.Requests;

/// <summary>
/// Represents a request to be sent to an API
/// </summary>
public interface IApiRequest
{
    IApiRequestSerializer? Serializer { get; set; }

    bool HasMessage { get; }

    IAuthorizationProvider? AuthorizationProvider { get; set; }

    IRetryPolicy[] RetryPolicies { get; set; }

    Action? OnBeforeSend { get; set; }
    ApiResponseVariationResolver? Resolver { get; set; }
}
