using Microsoft.EntityFrameworkCore;
using Back.Domain.Entities;

namespace Back.Infra.Database;

public class AppDbContext : DbContext
{
    public DbSet<Local> Locals { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Local>(entity =>
        {
            entity.Property(e => e.Cep).IsRequired();
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();
        });
    }
}