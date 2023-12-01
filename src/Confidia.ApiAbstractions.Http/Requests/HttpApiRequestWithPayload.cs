namespace Confidia.ApiAbstractions.Http.Requests;

public abstract class HttpApiRequestWithPayload(Uri uri, object payload, ApiResponseVariationResolver? resolver) : HttpApiRequestBase(uri, resolver)
{
    public override bool HasMessage => true;

    public object Payload { get; set; } = payload;
}
