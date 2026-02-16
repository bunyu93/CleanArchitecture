using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureTemplate.Persistence.Configurations;

public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        // TABLE
        builder.ToTable("weather_forecasts");

        // PRIMARY KEY
        builder.HasKey(i => i.Id).HasName("PK_WeatherForecast");

        // PROPERTIES
        builder.Property(i => i.Date)
            .HasColumnName("Date")
            .HasColumnType("DATE");

        builder.OwnsOne(i => i.Temperature, j =>
        {
            j.Property(x => x.Celsius)
                .HasColumnName("Celsius")
                .HasColumnType("INTEGER");

            j.Property(x => x.Fahrenheit)
                .HasColumnName("Fahrenheit")
                .HasColumnType("INTEGER");
        });

        builder.Property(i => i.Summary)
            .HasColumnName("Summary")
            .HasMaxLength(200);
    }
}