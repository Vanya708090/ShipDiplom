using Microsoft.EntityFrameworkCore;
using ShipDiplom.Database.Configuration;
using ShipDiplom.Models.Entities;

namespace ShipDiplom.Database;

public class AppDbContext : DbContext
{
    public DbSet<Pier> Piers { get; set; }
    public DbSet<Ship> Ships { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new PierConfiguration());
        modelBuilder.ApplyConfiguration(new ShipConfiguration());

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

    }
}
