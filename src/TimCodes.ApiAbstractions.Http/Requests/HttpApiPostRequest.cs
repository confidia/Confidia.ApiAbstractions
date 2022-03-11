namespace TimCodes.ApiAbstractions.Http.Requests;

public class HttpApiPostRequest : HttpApiRequestWithPayload
{
    public HttpApiPostRequest(Uri uri, object payload, ApiResponseVariationResolver resolver) : base(uri, payload, resolver)
    {
    }

    public override HttpMethod Method => HttpMethod.Post;
}
