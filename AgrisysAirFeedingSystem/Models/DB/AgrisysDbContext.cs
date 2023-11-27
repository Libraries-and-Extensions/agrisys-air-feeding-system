using System.Diagnostics.Metrics;
using AgrisysAirFeedingSystem.Models.DBModels;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace AgrisysAirFeedingSystem.Models.DB;

using Microsoft.EntityFrameworkCore;
using System;

public class AgrisysDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=.\\Database\\Agrisys.db");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entity>()
            .Property(b => b.EntityType).HasDefaultValue(EntityType.Unknown);
        
        modelBuilder.Entity<Event>()
            .Property(b => b.EditLevel).HasDefaultValue(EditLevel.Info);

        modelBuilder.Entity<Mixture>().HasOne<Silo>(m=>m.FirstSilo).WithMany().HasForeignKey(m => m.FirstSiloId);
        modelBuilder.Entity<Mixture>().HasOne<Silo>(m=>m.SecondSilo).WithMany().HasForeignKey(m => m.SecondSiloId);
    }  

    // Define your DbSet properties for each entity
    public DbSet<Farm> Farms { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Entity> Entities { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<SensorMeasurement> Measurements { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<FeedingTime> FeedingTimes { get; set; }
    public DbSet<Area> Areas { get; set; }
    public DbSet<Mixture> Mixtures { get; set; }
    public DbSet<Target> Target { get; set; }
    public DbSet<MixtureSetpoint> MixtureSetpoints { get; set; }
    public DbSet<Silo> Silos { get; set; }
}