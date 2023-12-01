using Confidia.ApiAbstractions.FaultTolerance;

namespace Confidia.ApiAbstractions;

public interface IApiClient
{
    string ApiIdentifier { get; }
    IApiRequestSerializer DefaultRequestSerlializer { get; set; }
    IApiResponseDeserializer DefaultResponeDeserlializer { get; set; }
    IAuthorizationProvider? DefaultAuthorizationProvider { get; set; }
    IRetryPolicy[] DefaultRetryPolicies { get; set; }

    Task<IApiResponse> SendAsync(IApiRequest request);
}
