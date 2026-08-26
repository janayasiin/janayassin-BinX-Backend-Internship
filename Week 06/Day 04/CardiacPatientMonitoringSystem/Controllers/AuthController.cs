using CardiacPatientMonitoringSystem.DTOs.Auth;
using CardiacPatientMonitoringSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CardiacPatientMonitoringSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new
        {
            message = "Patient registered successfully."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var token = await _authService.LoginAsync(request);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new
        {
            token
        });
    }
}