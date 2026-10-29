using GameCatalog.DAL.Data;
using GameCatalog.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.DAL;

public static class Seeder
{
    public static async Task SeedAsync(GameCatalogDbContext context)
    {
        var developers = new List<Developer>
        {
            new Developer { Name = "Valve", Country = "United States" },
            new Developer { Name = "Nintendo", Country = "Japan" },
            new Developer { Name = "Rockstar Games", Country = "United States" },
            new Developer { Name = "CD Projekt Red", Country = "Poland" },
            new Developer { Name = "FromSoftware", Country = "Japan" },
            new Developer { Name = "Bethesda Game Studios", Country = "United States" },
            new Developer { Name = "Mojang Studios", Country = "Sweden" },
            new Developer { Name = "Naughty Dog", Country = "United States" }
        };

        foreach (var developer in developers)
        {
            var existingDeveloper = await context.Developers
                .FirstOrDefaultAsync(d => d.Name == developer.Name);

            if (existingDeveloper == null)
            {
                context.Developers.Add(developer);
            }
        }

        await context.SaveChangesAsync();

        var valve = await context.Developers
            .FirstAsync(d => d.Name == "Valve");

        var nintendo = await context.Developers
            .FirstAsync(d => d.Name == "Nintendo");

        var rockstar = await context.Developers
            .FirstAsync(d => d.Name == "Rockstar Games");

        var cdProjektRed = await context.Developers
            .FirstAsync(d => d.Name == "CD Projekt Red");

        var fromSoftware = await context.Developers
            .FirstAsync(d => d.Name == "FromSoftware");

        var bethesda = await context.Developers
            .FirstAsync(d => d.Name == "Bethesda Game Studios");

        var mojang = await context.Developers
            .FirstAsync(d => d.Name == "Mojang Studios");

        var naughtyDog = await context.Developers
            .FirstAsync(d => d.Name == "Naughty Dog");

        var games = new List<Game>
        {
            new Game
            {
                Title = "Half-Life 2",
                ReleaseDate = new DateTime(2004, 11, 16),
                Price = 9.99m,
                DeveloperId = valve.Id
            },
            new Game
            {
                Title = "Portal 2",
                ReleaseDate = new DateTime(2011, 4, 19),
                Price = 9.99m,
                DeveloperId = valve.Id
            },
            new Game
            {
                Title = "The Legend of Zelda: Breath of the Wild",
                ReleaseDate = new DateTime(2017, 3, 3),
                Price = 59.99m,
                DeveloperId = nintendo.Id
            },
            new Game
            {
                Title = "Super Mario Odyssey",
                ReleaseDate = new DateTime(2017, 10, 27),
                Price = 59.99m,
                DeveloperId = nintendo.Id
            },
            new Game
            {
                Title = "Grand Theft Auto V",
                ReleaseDate = new DateTime(2013, 9, 17),
                Price = 29.99m,
                DeveloperId = rockstar.Id
            },
            new Game
            {
                Title = "The Witcher 3: Wild Hunt",
                ReleaseDate = new DateTime(2015, 5, 19),
                Price = 39.99m,
                DeveloperId = cdProjektRed.Id
            },
            new Game
            {
                Title = "Elden Ring",
                ReleaseDate = new DateTime(2022, 2, 25),
                Price = 59.99m,
                DeveloperId = fromSoftware.Id
            },
            new Game
            {
                Title = "The Elder Scrolls V: Skyrim",
                ReleaseDate = new DateTime(2011, 11, 11),
                Price = 39.99m,
                DeveloperId = bethesda.Id
            },
            new Game
            {
                Title = "Minecraft",
                ReleaseDate = new DateTime(2011, 11, 18),
                Price = 29.99m,
                DeveloperId = mojang.Id
            },
            new Game
            {
                Title = "The Last of Us",
                ReleaseDate = new DateTime(2013, 6, 14),
                Price = 59.99m,
                DeveloperId = naughtyDog.Id
            }
        };

        foreach (var game in games)
        {
            var exists = await context.Games
                .AnyAsync(g => g.Title == game.Title);

            if (!exists)
            {
                context.Games.Add(game);
            }
        }

        await context.SaveChangesAsync();
    }
}