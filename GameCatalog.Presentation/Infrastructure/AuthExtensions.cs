using System.Security.Claims;
using System.Text;
using GameCatalog.DAL.Data;
using GameCatalog.DAL.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace GameCatalog.Presentation.Infrastructure;

public static class AuthExtensions
{
    public static IServiceCollection AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Identity
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<GameCatalogDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IRoleValidator<IdentityRole>, GameCatalog.BLL.Validators.RoleValidator>();

        // JWT
        var jwtSettings = configuration.GetSection("Jwt");
        var jwtKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key missing.");
        var issuer = jwtSettings["Issuer"] ?? throw new InvalidOperationException("JWT Issuer missing.");
        var audience = jwtSettings["Audience"] ?? throw new InvalidOperationException("JWT Audience missing.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                RoleClaimType = ClaimTypes.Role,
                NameClaimType = ClaimTypes.Name
            };
        });

        return services;
    }
}