namespace TimCodes.ApiAbstractions.Http.Responses;

public class HttpApiGenericResponse<TContent>(HttpResponseMessage response) : HttpApiResponseBase(response), IDisposable
{
    public TContent? Content { get; set; }

    public Type ContentType => typeof(TContent); 

    public override void Dispose()
    {
        base.Dispose();
        Content = default;
    }
}
