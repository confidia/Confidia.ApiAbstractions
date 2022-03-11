namespace TimCodes.ApiAbstractions.Http.Requests;

public class HttpApiPatchRequest : HttpApiRequestWithPayload
{
    public HttpApiPatchRequest(Uri uri, object payload, ApiResponseVariationResolver resolver) : base(uri, payload, resolver)
    {
    }

    public override HttpMethod Method => HttpMethod.Patch;
}
