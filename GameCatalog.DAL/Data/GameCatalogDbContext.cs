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
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map DateOfBirth correctly for Postgres
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.DateOfBirth)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<Game>()
            .Property(g => g.ReleaseDate)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<IdentityRole>()
            .HasIndex(r => r.NormalizedName)
            .IsUnique();

        modelBuilder.Entity<Developer>()
            .HasMany(d => d.Games)
            .WithOne(g => g.Developer)
            .HasForeignKey(g => g.DeveloperId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IdentityUserRole<string>>(b =>
        {
            b.HasOne<IdentityRole>()
                .WithMany()
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Game>()
            .HasMany(g => g.Genres)
            .WithMany(g => g.Games)
            .UsingEntity(j => j.ToTable("GameGenres"));
    }
}