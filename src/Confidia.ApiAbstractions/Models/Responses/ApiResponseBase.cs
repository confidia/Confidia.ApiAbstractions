namespace Confidia.ApiAbstractions.Models.Responses;

public abstract class ApiResponseBase : IApiResponse
{
    public bool Success { get; protected init; }

    public virtual void Dispose() => GC.SuppressFinalize(this);
}
