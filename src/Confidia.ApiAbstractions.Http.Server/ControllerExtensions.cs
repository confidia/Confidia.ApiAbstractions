using Microsoft.AspNetCore.Mvc;
using Confidia.ApiAbstractions.Http.Responses;

namespace Confidia.ApiAbstractions.Http.Server;

public static class ControllerExtensions
{
    public static IActionResult HttpApiResult(this ControllerBase controller, HttpApiMessageBase payload)
    {
        var status = payload.GetStatusCodeIntendedForServerToReturn();

        return controller.StatusCode((int)status, payload);
    }
}
