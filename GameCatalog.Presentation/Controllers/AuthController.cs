using GameCatalog.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public record AuthRequest(string Email, string Password);

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(AuthRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Email and password are required." });

        var result = await _authService.RegisterAsync(request.Email, request.Password);

        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return Ok(new
        {
            message = "Registration successful.",
            token = result.Token,
            expiresAt = result.ValidTo
        });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Email and password are required." });

        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(new
        {
            token = result.Token,
            expiresAt = result.ValidTo
        });
    }
}