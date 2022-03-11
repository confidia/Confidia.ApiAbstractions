namespace TimCodes.ApiAbstractions.Http.Requests;

public class HttpApiDeleteRequest : HttpApiRequestBase
{
    public HttpApiDeleteRequest(Uri uri, ApiResponseVariationResolver resolver) : base(uri, resolver)
    {
    }

    public override HttpMethod Method => HttpMethod.Delete;

    public override bool HasMessage => false;
}
