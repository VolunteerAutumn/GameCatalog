using AutoMapper;
using FluentValidation;
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

        services.AddAutoMapper(
            cfg => { },
            typeof(Mapping.DeveloperProfile).Assembly);

        services.AddValidatorsFromAssemblyContaining<
            Validators.DeveloperCreateValidator>();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<GameCatalogDbContext>();

        services.AddScoped<Interfaces.IRoleService, Services.RoleService>();

        return services;
    }
}