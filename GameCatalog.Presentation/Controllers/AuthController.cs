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
    public record AssignRoleRequest(string Email, string RoleName);

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(AuthRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "Email and password are required." });

        var result = await _authService.RegisterAsync(request.Email, request.Password);

        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return Ok(new { message = "Registration successful. Please check your email to confirm your account." });
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(AuthRequest request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid email/password, OR email not confirmed." });

        return Ok(new { token = result.Token, expiresAt = result.ValidTo });
    }

    [HttpGet("confirm-email")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
    {
        var result = await _authService.ConfirmEmailAsync(userId, token);

        if (!result)
            return BadRequest(new { message = "Invalid or expired token." });

        return Ok(new { message = "Email confirmed successfully. You can now log in." });
    }

    [HttpPost("assign-role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(AssignRoleRequest request)
    {
        var result = await _authService.AssignRoleAsync(request.Email, request.RoleName);

        if (!result)
            return BadRequest(new { message = "Failed to assign role. User or Role does not exist." });

        return Ok(new { message = $"Role {request.RoleName} assigned to {request.Email}." });
    }

    public record ChangePasswordRequest(string Email, string CurrentPassword, string NewPassword);
    public record ForgotPasswordRequest(string Email);
    public record ResetPasswordRequest(string Email, string Token, string NewPassword);
    public record ChangeEmailRequest(string CurrentEmail, string NewEmail);

    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        var result = await _authService.ChangePasswordAsync(request.Email, request.CurrentPassword, request.NewPassword);
        if (!result) return BadRequest(new { message = "Failed to change password. Check your current password." });
        return Ok(new { message = "Password changed successfully." });
    }

    [HttpPost("forgot-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        await _authService.ForgotPasswordAsync(request.Email);
        return Ok(new { message = "If that email exists, a reset link has been sent." });
    }

    [HttpGet("reset-password")]
    [AllowAnonymous]
    public IActionResult ResetPasswordGet([FromQuery] string email, [FromQuery] string token)
    {
        // Since we can't show a frontend form, we return a JSON message explaining how to finish.
        return Ok(new
        {
            message = "Token received successfully. Please use the POST /api/Auth/reset-password endpoint in Swagger to set your new password.",
            email = email,
            token = token
        });
    }

    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);
        if (!result) return BadRequest(new { message = "Failed to reset password. Token may be invalid or expired." });
        return Ok(new { message = "Password reset successfully. You can now log in." });
    }

    [HttpPost("request-email-change")]
    [Authorize]
    public async Task<IActionResult> RequestEmailChange(ChangeEmailRequest request)
    {
        var result = await _authService.RequestEmailChangeAsync(request.CurrentEmail, request.NewEmail);
        if (!result.Succeeded) return BadRequest(new { message = "User not found." });
        return Ok(new { message = "A confirmation link has been sent to your new email address." });
    }

    [HttpGet("confirm-email-change")]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmailChange([FromQuery] string userId, [FromQuery] string newEmail, [FromQuery] string token)
    {
        var result = await _authService.ConfirmEmailChangeAsync(userId, newEmail, token);
        if (!result) return BadRequest(new { message = "Invalid or expired token." });
        return Ok(new { message = "Email changed successfully. You can now log in with your new email." });
    }
}