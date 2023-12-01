namespace TimCodes.ApiAbstractions.Http.Requests;

public class HttpApiDeleteRequest(Uri uri, ApiResponseVariationResolver resolver) : HttpApiRequestBase(uri, resolver)
{
    public override HttpMethod Method => HttpMethod.Delete;

    public override bool HasMessage => false;
}
