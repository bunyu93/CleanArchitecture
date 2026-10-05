using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitectureTemplate.Persistence.Context;

public class EfDbContext(DbContextOptions<EfDbContext> options) : DbContext(options)
{
    public virtual DbSet<WeatherForecast> WeatherForecast => Set<WeatherForecast>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("weather");

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new WeatherForecastConfiguration());
    }
}
