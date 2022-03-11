using Microsoft.AspNetCore.Mvc;
using System.Net;
using TimCodes.ApiAbstractions.Http.SampleServer.Models;
using TimCodes.ApiAbstractions.Http.Server;

namespace TimCodes.ApiAbstractions.Http.SampleServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("fail")]
    public IActionResult Fail()
    {
        var message = HttpApiMessageBuilder.CreateFailure(HttpStatusCode.PaymentRequired, "Payment required", ApiError.SomethingWrong);

        return this.HttpApiResult(message);
    }

    [HttpGet("success")]
    public IActionResult Success()
    {
        var message = HttpApiMessageBuilder.CreateSuccess();

        return this.HttpApiResult(message);
    }
}
