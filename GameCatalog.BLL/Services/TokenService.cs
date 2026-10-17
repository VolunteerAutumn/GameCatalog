using GameCatalog.BLL.Interfaces;
using GameCatalog.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameCatalog.BLL.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<(string Token, DateTime ValidTo)> CreateTokenAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, user.Id),
            new(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Email ?? "")
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var jwtSettings = _configuration.GetSection("Jwt");

        var key = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key missing.");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer missing.");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience missing.");

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return (new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
    }
}