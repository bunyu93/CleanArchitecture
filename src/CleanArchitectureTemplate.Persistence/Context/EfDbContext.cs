using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Persistence.Configurations;
using CleanArchitectureTemplate.Persistence.Settings.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanArchitectureTemplate.Persistence.Context;

public class EfDbContext(
    DbContextOptions options,
    IOptions<DatabaseOptions> databaseOptions
) : DbContext(options)
{
    private readonly IOptions<DatabaseOptions> _databaseOptions = databaseOptions;

    public virtual DbSet<WeatherForecast> WeatherForecast => Set<WeatherForecast>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseOptions.Value.Connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Write Fluent API configurations here
        //Property Configurations

        // Other databases support the default schema
        modelBuilder.HasDefaultSchema("weather");

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new WeatherForecastConfiguration());
    }
}