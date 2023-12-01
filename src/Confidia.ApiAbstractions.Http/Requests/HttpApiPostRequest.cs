namespace Confidia.ApiAbstractions.Http.Requests;

public class HttpApiPostRequest(Uri uri, object payload, ApiResponseVariationResolver? resolver) : HttpApiRequestWithPayload(uri, payload, resolver)
{
    public override HttpMethod Method => HttpMethod.Post;
}
