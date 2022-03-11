namespace TimCodes.ApiAbstractions.Http.Responses;

/// <summary>
/// Represents a payload that can be returned from a custom HTTP API
/// </summary>
public class HttpApiMessageBase : ApiMessageBase
{
    internal HttpStatusCode StatusCodeToReturn { get; set; } = HttpStatusCode.BadRequest;
}
