namespace TimCodes.ApiAbstractions.Http.Responses;

public class HttpApiEmptyResponse : HttpApiResponseBase
{
    public HttpApiEmptyResponse(HttpResponseMessage response) : base(response)
    {
        Success = response.IsSuccessStatusCode;
    }
}
