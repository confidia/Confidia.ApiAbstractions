using System.Net;
using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.Server;

public static class ApiMessageBaseExtensions
{
    /// <summary>
    /// Used when returning a payload from a server to determine the correct status to set with it. 
    /// Note this value is INCORRECT when called on the client side as it will always default to BadRequest.
    /// </summary>
    /// <returns></returns>
    public static HttpStatusCode GetStatusCodeIntendedForServerToReturn(this HttpApiMessageBase message) => message.StatusCodeToReturn;
}
