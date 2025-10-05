using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.Presentation.Api.Controllers;

[Route("[controller]")]
public class HealthCheckController : ApiBaseController
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("CleanArchitecture.Presentation.Api is healthy");
    }
}