using Microsoft.EntityFrameworkCore;
using Back.Domain.Entities;

namespace Back.Infra.Database;

public class AppDbContext : DbContext
{

    public DbSet<User> Users { get; set; }
    public DbSet<Distance> Distances { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public static DbContextOptions<AppDbContext> CreateOptions(
        string DbHost,
        string DbName,
        string DbUser,
        string DbPassword,
        string DbPort = "5432"
    ){
        string connectionString =
            $"Host={DbHost};" +
            $"Database={DbName};" +
            $"Username={DbUser};" +
            $"Password={DbPassword}";

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql("")
            .Options;

        return options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(e => e.Username).IsRequired();
            entity.Property(e => e.Email).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<Distance>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(e => e.De).IsRequired();
            entity.Property(e => e.Para).IsRequired();
            entity.Property(e => e.Distancia).IsRequired();
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);

            entity.HasIndex(e => new { e.De, e.Para, e.UserId }).IsUnique();
        });
    }
}