using Microsoft.AspNetCore.Mvc;

namespace Chroma.Presentation.Api.Controllers;

[Route("[controller]")]
public class HealthCheckController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Chroma.Presentation.Api is healthy");
    }
}