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

    // Add this inside OnModelCreating():
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>()
            .Property(g => g.ReleaseDate)
            .HasColumnType("timestamp without time zone");

        modelBuilder.Entity<IdentityRole>()
            .HasIndex(r => r.NormalizedName)
            .IsUnique();

        // Restrict deleting a developer that still has games.
        modelBuilder.Entity<Developer>()
            .HasMany(d => d.Games)
            .WithOne(g => g.Developer)
            .HasForeignKey(g => g.DeveloperId)
            .OnDelete(DeleteBehavior.Restrict);

        // Restrict deleting a role that is still assigned to users.
        // (Identity's default for the RoleId FK would otherwise cascade-remove
        // user-role links, silently stripping roles from users.)
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
            .UsingEntity(j => j.ToTable("GameGenres")); // EF Core will automatically make the join table!
    }
}