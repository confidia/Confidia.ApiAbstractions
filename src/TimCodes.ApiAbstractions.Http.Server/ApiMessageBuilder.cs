using System.Net;
using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.Server;

public static class HttpApiMessageBuilder
{
    public static HttpApiMessageBase CreateSuccess()
        => CreateSuccess<HttpApiMessageBase>();

    public static HttpApiMessageBase CreateFailure(HttpStatusCode statusCode, string? message, dynamic errorCode)
        => CreateFailure<HttpApiMessageBase>(statusCode, message, errorCode);

    public static TMessage CreateFailure<TMessage>(HttpStatusCode statusCode, string? message, dynamic errorCode)
        where TMessage : HttpApiMessageBase, new()
    {
        return errorCode is not Enum ? 
            throw new ArgumentException("Error code must be an enum", nameof(errorCode)) : 
            (new()
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = (int?)errorCode,
                StatusCodeToReturn = statusCode
            });
    }

    public static TMessage CreateSuccess<TMessage>()
        where TMessage : HttpApiMessageBase, new() => new()
        {
            IsSuccess = true,
            StatusCodeToReturn = HttpStatusCode.OK
        };
}
