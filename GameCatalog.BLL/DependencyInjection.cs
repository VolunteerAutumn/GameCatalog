using AutoMapper;
using FluentValidation;
using GameCatalog.BLL.Validators;
using GameCatalog.DAL;
using GameCatalog.DAL.Data;
using GameCatalog.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameCatalog.BLL;

public static class DependencyInjection
{
    public static IServiceCollection AddBll(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDal(configuration);

        services.AddScoped<
            Interfaces.IDeveloperService,
            Services.DeveloperService>();

        services.AddScoped<
            Interfaces.IGameService,
            Services.GameService>();

        // 1. Put the empty lambda back so AutoMapper 15 hits the correct overload
        services.AddAutoMapper(
            cfg => { },
            typeof(Mapping.DeveloperProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<
            Validators.DeveloperCreateValidator>();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<GameCatalogDbContext>();

        // 2. Because we used 'using GameCatalog.BLL.Validators;' at the top,
        // we can now use the clean name WITHOUT the generic <IdentityRole> suffix,
        // and the compiler will correctly pick YOUR class instead of Microsoft's.
        services.AddScoped<
            IRoleValidator<IdentityRole>,
            RoleValidator>();

        services.AddScoped<Interfaces.IRoleService, Services.RoleService>();

        services.AddScoped<Interfaces.ITokenService, Services.TokenService>();
        services.AddScoped<Interfaces.IAuthService, Services.AuthService>();
        services.AddScoped<Interfaces.IGenreService, Services.GenreService>();

        return services;
    }
}