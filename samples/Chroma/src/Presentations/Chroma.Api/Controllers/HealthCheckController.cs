using Microsoft.AspNetCore.Mvc;

namespace Chroma.Api.Controllers;

[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Chroma.Api is healthy");
    }
}