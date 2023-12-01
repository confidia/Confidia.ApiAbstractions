namespace TimCodes.ApiAbstractions.Http.Responses;

public class HttpApiValidationResponse(HttpResponseMessage response) : HttpApiResponseBase(response), IDisposable
{
    public DotNetValidationProblem? Content { get; set; }
}
