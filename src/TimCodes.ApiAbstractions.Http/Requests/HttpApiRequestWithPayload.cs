namespace TimCodes.ApiAbstractions.Http.Requests;

public abstract class HttpApiRequestWithPayload : HttpApiRequestBase
{
    public HttpApiRequestWithPayload(Uri uri, object payload, ApiResponseVariationResolver resolver) : base(uri, resolver)
    {
        Payload = payload;
    }

    public override bool HasMessage => true;

    public object Payload { get; set; }
}
