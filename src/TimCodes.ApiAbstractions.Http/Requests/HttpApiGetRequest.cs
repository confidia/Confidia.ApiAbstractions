namespace TimCodes.ApiAbstractions.Http.Requests;

public class HttpApiGetRequest(Uri uri, ApiResponseVariationResolver resolver) : HttpApiRequestBase(uri, resolver)
{
    public override HttpMethod Method => HttpMethod.Get;

    public override bool HasMessage => false;
}
