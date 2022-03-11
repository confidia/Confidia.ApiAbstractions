namespace TimCodes.ApiAbstractions.Http.Requests;

public class HttpApiGetRequest : HttpApiRequestBase
{
    public HttpApiGetRequest(Uri uri, ApiResponseVariationResolver resolver) : base(uri, resolver)
    {
    }

    public override HttpMethod Method => HttpMethod.Get;

    public override bool HasMessage => false;
}
