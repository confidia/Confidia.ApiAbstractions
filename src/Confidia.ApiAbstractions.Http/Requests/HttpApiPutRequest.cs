namespace Confidia.ApiAbstractions.Http.Requests;

public class HttpApiPutRequest(Uri uri, object payload, ApiResponseVariationResolver resolver) : HttpApiRequestWithPayload(uri, payload, resolver)
{
    public override HttpMethod Method => HttpMethod.Put;
}
