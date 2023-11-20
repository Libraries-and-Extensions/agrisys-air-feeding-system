using AgrisysAirFeedingSystem.Models.DBModels;

namespace AgrisysAirFeedingSystem.Models.DB;

using Microsoft.EntityFrameworkCore;
using System;

public class AgrisysDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Fetch the connection string from environment variables
            string connectionString = Environment.GetEnvironmentVariable("YOUR_CONNECTION_STRING_VARIABLE");

            // Use the retrieved connection string to configure the DbContext
            optionsBuilder.UseSqlServer(connectionString); // Change this according to your database provider
        }
    }

    // Define your DbSet properties for each entity
    public DbSet<Farm> Farms { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Entity> Entities { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<DBModels.Data> Data { get; set; }
    public DbSet<Event> Events { get; set; }

    // Your other configurations, models, etc. go here...
}