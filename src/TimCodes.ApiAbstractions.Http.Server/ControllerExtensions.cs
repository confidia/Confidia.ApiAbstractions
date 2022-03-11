using Microsoft.AspNetCore.Mvc;
using TimCodes.ApiAbstractions.Http.Responses;

namespace TimCodes.ApiAbstractions.Http.Server;

public static class ControllerExtensions
{
    public static IActionResult HttpApiResult(this ControllerBase controller, HttpApiMessageBase payload)
    {
        var status = payload.GetStatusCodeIntendedForServerToReturn();

        return controller.StatusCode((int)status, payload);
    }
}
