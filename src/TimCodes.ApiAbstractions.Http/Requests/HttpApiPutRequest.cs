namespace TimCodes.ApiAbstractions.Http.Requests;

public class HttpApiPutRequest : HttpApiRequestWithPayload
{
    public HttpApiPutRequest(Uri uri, object payload, ApiResponseVariationResolver resolver) : base(uri, payload, resolver)
    {
    }

    public override HttpMethod Method => HttpMethod.Put;
}
