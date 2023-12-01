using System.Net;
using Confidia.ApiAbstractions.Http.Responses;

namespace Confidia.ApiAbstractions.Http.Server;

public static class HttpApiMessageBuilder
{
    public static HttpApiMessageBase CreateSuccess(Action<HttpApiMessageBase>? alter = null)
        => CreateSuccess<HttpApiMessageBase>(alter);

    public static HttpApiMessageBase CreateFailure(HttpStatusCode statusCode, string? message, dynamic errorCode, Action<HttpApiMessageBase>? alter = null)
        => CreateFailure<HttpApiMessageBase>(statusCode, message, errorCode, alter);

    public static TMessage CreateFailure<TMessage>(HttpStatusCode statusCode, string? message, dynamic errorCode, Action<TMessage>? alter = null)
        where TMessage : HttpApiMessageBase, new() 
        => errorCode is not Enum ?
            throw new ArgumentException("Error code must be an enum", nameof(errorCode)) :
            Alter(new()
            {
                IsSuccess = false,
                Message = message,
                ErrorCode = (int?)errorCode,
                StatusCodeToReturn = statusCode
            }, alter);

    public static TMessage CreateSuccess<TMessage>(Action<TMessage>? alter = null)
        where TMessage : HttpApiMessageBase, new() 
        => Alter(new()
           {
               IsSuccess = true,
               StatusCodeToReturn = HttpStatusCode.OK
           }, alter);

    private static HttpApiMessageBase Alter(HttpApiMessageBase message, Action<HttpApiMessageBase>? alter)
    {
        alter?.Invoke(message);
        return message;
    }

    private static TMessage Alter<TMessage>(TMessage message, Action<TMessage>? alter)
        where TMessage : HttpApiMessageBase, new()
    {
        alter?.Invoke(message);
        return message;
    }
}
