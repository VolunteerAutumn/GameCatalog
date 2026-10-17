using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using Microsoft.AspNetCore.Identity;

namespace GameCatalog.BLL.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<(bool Succeeded, string[] Errors, string? Token, DateTime? ValidTo)> RegisterAsync(string email, string password)
    {
        var user = new ApplicationUser { UserName = email, Email = email };
        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            return (false, result.Errors.Select(e => e.Description).ToArray(), null, null);

        // Assign default role
        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user); // rollback
            return (false, roleResult.Errors.Select(e => e.Description).ToArray(), null, null);
        }

        var (token, validTo) = await _tokenService.CreateTokenAsync(user);
        return (true, Array.Empty<string>(), token, validTo);
    }

    public async Task<(bool Succeeded, string? Token, DateTime? ValidTo)> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            return (false, null, null);

        // Give roleless users the default role
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Count == 0)
        {
            await _userManager.AddToRoleAsync(user, "User");
        }

        var (token, validTo) = await _tokenService.CreateTokenAsync(user);
        return (true, token, validTo);
    }
}