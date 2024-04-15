﻿using Microsoft.EntityFrameworkCore;
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
    }
}
