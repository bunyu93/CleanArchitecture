using CleanArchitectureTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitectureTemplate.Persistence.Configurations
{
    public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
    {
        public void Configure(EntityTypeBuilder<WeatherForecast> modelBuilder)
        {
            // TABLE
            modelBuilder.ToTable("WeatherForecast");

            // PRIMARY KEY
            modelBuilder.HasKey(i => i.Id).HasName("PK_WeatherForecast");

            // PROPERTIES
            modelBuilder.Property(i => i.Date)
              .HasColumnName("Date")
              .HasColumnType("DATE");

            modelBuilder.OwnsOne(i => i.Temperature, j =>
            {
                j.Property(x => x.Celsius)
                    .HasColumnName("Celsius")
                    .HasColumnType("INTEGER");

                j.Property(x => x.Fahrenheit)
                    .HasColumnName("Fahrenheit")
                    .HasColumnType("INTEGER");
            });

            modelBuilder.Property(i => i.Summary)
                .HasColumnName("Summary")
                .HasColumnType("STRING");
        }
    }
}