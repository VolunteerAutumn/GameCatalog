using GameCatalog.DAL;
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

        return services;
    }
}