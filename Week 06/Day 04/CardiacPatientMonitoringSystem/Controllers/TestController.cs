using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class TestController : ControllerBase
{
    [HttpGet("error")]
    public IActionResult TestError()
    {
        throw new Exception("This is a secret internal error.");
    }
}