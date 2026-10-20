using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Web; // For URL encoding the token

namespace GameCatalog.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IEmailService _emailService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IEmailService emailService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _emailService = emailService;
    }

    public async Task<(bool Succeeded, string[] Errors)> RegisterAsync(string email, string password)
    {
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description).ToArray());

        await _userManager.AddToRoleAsync(user, "User");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = HttpUtility.UrlEncode(token);

        var confirmationLink = $"http://localhost:5000/api/Auth/confirm-email?userId={user.Id}&token={encodedToken}";

        var emailBody = $"Please confirm your email by clicking this link: <a href='{confirmationLink}'>Confirm Email</a>";

        await _emailService.SendEmailAsync(user.Email!, "Confirm your email", emailBody);

        return (true, Array.Empty<string>());
    }

    public async Task<(bool Succeeded, string? Token, DateTime? ValidTo)> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            return (false, null, null);

        // Block login if email is not confirmed
        if (!await _userManager.IsEmailConfirmedAsync(user))
            return (false, null, null);

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count == 0)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        var (token, validTo) = await _tokenService.CreateTokenAsync(user);
        return (true, token, validTo);
    }

    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var decodedToken = System.Web.HttpUtility.UrlDecode(token);
        var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

        if (!result.Succeeded)
        {
            var fixedToken = token.Replace(" ", "+");
            var decodedFixedToken = System.Web.HttpUtility.UrlDecode(fixedToken);
            result = await _userManager.ConfirmEmailAsync(user, decodedFixedToken);
        }

        return result.Succeeded;
    }

    public async Task<bool> AssignRoleAsync(string email, string roleName)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        // Check if role exists
        var roleExists = await _userManager.GetRolesAsync(user);
        if (roleExists.Contains(roleName)) return true; // Already has it

        var result = await _userManager.AddToRoleAsync(user, roleName);
        return result.Succeeded;
    }

    public async Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return result.Succeeded;
    }

    public async Task<bool> ForgotPasswordAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return true; // Don't reveal if user exists

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var encodedToken = System.Web.HttpUtility.UrlEncode(token);

        var resetLink = $"http://localhost:5000/api/Auth/reset-password?email={email}&token={encodedToken}";
        var emailBody = $"Please reset your password by clicking this link: <a href='{resetLink}'>Reset Password</a>";

        await _emailService.SendEmailAsync(email, "Reset your password", emailBody);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        // 1. Initial decode
        var decodedToken = System.Web.HttpUtility.UrlDecode(token);
        var result = await _userManager.ResetPasswordAsync(user, decodedToken, newPassword);

        // 2. Bulletproof fallback for spaces/broken URLs
        if (!result.Succeeded)
        {
            var fixedToken = token.Replace(" ", "+");
            fixedToken = System.Uri.UnescapeDataString(fixedToken);
            result = await _userManager.ResetPasswordAsync(user, fixedToken, newPassword);
        }

        return result.Succeeded;
    }

    public async Task<(bool Succeeded, string? Token)> RequestEmailChangeAsync(string currentEmail, string newEmail)
    {
        var user = await _userManager.FindByEmailAsync(currentEmail);
        if (user == null) return (false, null);

        var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        var encodedToken = System.Web.HttpUtility.UrlEncode(token);

        // The link goes to the NEW email address
        var confirmationLink = $"http://localhost:5000/api/Auth/confirm-email-change?userId={user.Id}&newEmail={newEmail}&token={encodedToken}";
        var emailBody = $"Please confirm your new email by clicking this link: <a href='{confirmationLink}'>Confirm New Email</a>";

        await _emailService.SendEmailAsync(newEmail, "Confirm your new email", emailBody);

        return (true, token);
    }

    public async Task<bool> ConfirmEmailChangeAsync(string userId, string newEmail, string token)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        // 1. Decode the URL (turns %3d back into =, etc.)
        var decodedToken = System.Web.HttpUtility.UrlDecode(token);

        // 2. Try to change the email
        var result = await _userManager.ChangeEmailAsync(user, newEmail, decodedToken);

        // 3. If it fails, it's because the browser messed up the + or / characters. Let's fix them manually.
        if (!result.Succeeded)
        {
            // Browsers turn '+' into ' ' (spaces). We swap them back.
            var fixedToken = token.Replace(" ", "+");

            // Sometimes %2B gets double-encoded, let's unescape it completely
            fixedToken = System.Uri.UnescapeDataString(fixedToken);

            result = await _userManager.ChangeEmailAsync(user, newEmail, fixedToken);
        }

        // 4. Update Username
        if (result.Succeeded)
        {
            await _userManager.SetUserNameAsync(user, newEmail);
        }

        return result.Succeeded;
    }
}