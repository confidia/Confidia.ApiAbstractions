namespace TimCodes.ApiAbstractions.Http.Requests;

public class HttpApiPatchRequest(Uri uri, object payload, ApiResponseVariationResolver resolver) : HttpApiRequestWithPayload(uri, payload, resolver)
{
    public override HttpMethod Method => HttpMethod.Patch;
}
