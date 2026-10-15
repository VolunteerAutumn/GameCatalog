using GameCatalog.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameCatalog.DAL.Data;

public class GameCatalogDbContext : IdentityDbContext<ApplicationUser>
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
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>()
            .Property(g => g.ReleaseDate)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<IdentityRole>()
            .HasIndex(r => r.NormalizedName)
            .IsUnique();
    }
}