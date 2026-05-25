using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.WebHost.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<object> Get()
        => Ok(new { status = "ok" });
}
