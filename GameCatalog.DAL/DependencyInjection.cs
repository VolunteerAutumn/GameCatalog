using GameCatalog.DAL.Data;
using GameCatalog.DAL.Interfaces;
using GameCatalog.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameCatalog.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDal(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<GameCatalogDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IDeveloperRepository, DeveloperRepository>();
        services.AddScoped<IGameRepository, GameRepository>();

        return services;
    }
}