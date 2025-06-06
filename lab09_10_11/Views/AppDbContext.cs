using lab09.Models;

namespace lab09.Views;

using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Uzytkownik> Loginy { get; set; }
    public DbSet<Act> Acts { get; set; }
    public DbSet<Layer> Layers { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<Enemy> Enemies { get; set; }
    public DbSet<EnemiesLevels> EnemiesLevels { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Composite key for join table
        modelBuilder.Entity<EnemiesLevels>()
            .HasKey(el => new { el.LevelId, el.EnemyId });

        modelBuilder.Entity<EnemiesLevels>()
            .HasOne(el => el.Level)
            .WithMany(l => l.EnemiesLevels)
            .HasForeignKey(el => el.LevelId);

        modelBuilder.Entity<EnemiesLevels>()
            .HasOne(el => el.Enemy)
            .WithMany(e => e.EnemiesLevels)
            .HasForeignKey(el => el.EnemyId);
    }
}


