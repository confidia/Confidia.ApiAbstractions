namespace TimCodes.ApiAbstractions.Http.Responses;

public class HttpApiValidationResponse : HttpApiResponseBase, IDisposable
{
    public HttpApiValidationResponse(HttpResponseMessage response) : base(response)
    {
    }

    public DotNetValidationProblem? Content { get; set; }
}
