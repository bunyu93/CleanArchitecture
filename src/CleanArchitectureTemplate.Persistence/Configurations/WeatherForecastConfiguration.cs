using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureTemplate.Persistence.Configurations;

public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        // TABLE
        builder.ToTable("forecast");

        // PRIMARY KEY
        builder.HasKey(i => i.Id).HasName("pk_forecast");

        // PROPERTIES
        builder.Property(x => x.Id)
            .HasColumnName("id")
            .HasColumnType("Integer")
            .IsRequired();

        builder.Property(i => i.Date)
            .HasColumnName("date")
            .HasColumnType("Date");

        builder.OwnsOne(i => i.Temperature, j =>
        {
            j.Property(x => x.Celsius)
                .HasColumnName("celsius")
                .HasColumnType("Integer");

            j.Property(x => x.Fahrenheit)
                .HasColumnName("fahrenheit")
                .HasColumnType("Integer");
        });

        builder.Property(i => i.Summary)
            .HasColumnName("summary")
            .HasColumnType("Text")
            .HasMaxLength(200);
    }
}