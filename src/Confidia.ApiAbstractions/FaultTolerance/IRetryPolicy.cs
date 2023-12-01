namespace Confidia.ApiAbstractions.FaultTolerance;

public interface IRetryPolicy
{
    bool ShouldRetry(IApiResponse response, int attempt);

    Task OnBeforeRetryAsync(IApiRequest request, IApiResponse response, string apiIdentifier);
}
