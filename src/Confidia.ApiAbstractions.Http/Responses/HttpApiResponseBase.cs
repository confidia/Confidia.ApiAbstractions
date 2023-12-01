namespace Confidia.ApiAbstractions.Http.Responses;

public abstract class HttpApiResponseBase : ApiResponseBase
{
    public HttpApiResponseBase(HttpResponseMessage response)
    {
        RawMessage = response;
        Success = RawMessage.IsSuccessStatusCode;
    }

    public HttpStatusCode StatusCode => RawMessage.StatusCode;

    public HttpResponseMessage RawMessage { get; }

    public override void Dispose() 
    { 
        RawMessage?.Dispose();
        GC.SuppressFinalize(this);
    }
}
