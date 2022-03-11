namespace TimCodes.ApiAbstractions.Http.Responses;

public class HttpApiGenericResponse<TContent> : HttpApiResponseBase, IDisposable
{
    public HttpApiGenericResponse(HttpResponseMessage response) : base(response)
    {
    }

    public TContent? Content { get; set; }

    public Type ContentType => typeof(TContent); 

    public override void Dispose()
    {
        base.Dispose();
        Content = default;
    }
}
