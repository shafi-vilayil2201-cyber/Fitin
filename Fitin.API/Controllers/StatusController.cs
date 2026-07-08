using Microsoft.AspNetCore.Mvc;

namespace Fitin.API.Controllers;

[ApiController]
[Route("api/status")]
public class StatusController : BaseApiController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Success(new
        {
            status = "ok",
            timestampUtc = DateTime.UtcNow
        }, "API is running");
    }
}
