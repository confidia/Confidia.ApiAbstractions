namespace TimCodes.ApiAbstractions.Models.Responses;

public class ErrorApiResponse : ApiResponseBase
{
    public Exception? Exception { get; set; }
}
