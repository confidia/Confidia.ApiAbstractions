using TimCodes.ApiAbstractions.FaultTolerance;

namespace TimCodes.ApiAbstractions;

public interface IApiClient
{
    string ApiIdentifier { get; }
    IApiRequestSerializer DefaultRequestSerlializer { get; set; }
    IApiResponseDeserializer DefaultResponeDeserlializer { get; set; }
    IAuthorizationProvider? DefaultAuthorizationProvider { get; set; }
    IRetryPolicy[] DefaultRetryPolicies { get; set; }

    Task<IApiResponse> SendAsync(IApiRequest request);
}
