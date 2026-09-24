using GameCatalog.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.DAL.Data;

public class GameCatalogDbContext : DbContext
{
    public GameCatalogDbContext(
        DbContextOptions<GameCatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Developer> Developers { get; set; }

    public DbSet<Game> Games { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>()
            .Property(g => g.ReleaseDate)
            .HasColumnType("timestamp without time zone");
    }
}