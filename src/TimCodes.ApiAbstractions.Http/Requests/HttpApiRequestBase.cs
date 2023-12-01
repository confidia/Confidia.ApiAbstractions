namespace TimCodes.ApiAbstractions.Http.Requests;

public abstract class HttpApiRequestBase : ApiRequestBase
{
    public HttpApiRequestBase(Uri uri, ApiResponseVariationResolver? resolver)
    {
        Uri = uri;
        Resolver = resolver;
    }

    public Uri Uri { get; }
    public abstract HttpMethod Method { get; }

    public HttpRequestMessage? Message { get; internal set; }
}
