using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Persistence.Context;
using CleanArchitectureTemplate.Persistence.Settings.Options;
using CleanArchitectureTemplate.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanArchitectureTemplate.Test.DatabaseIntegrationTests;

[Collection("Database")]
public class DatabaseIntegrationTest(DockerPostgresTestFixture fixture)
{
    private EfDbContext CreateDbContext()
    {
        DbContextOptions<EfDbContext> options = new DbContextOptionsBuilder<EfDbContext>().Options;
        IOptions<DatabaseOptions> dbOptions =
            Options.Create(new DatabaseOptions { Connection = fixture.ConnectionString });
        return new EfDbContext(options, dbOptions);
    }

    [Fact]
    public async Task Should_Connect_To_PostgreSQL_Container()
    {
        // Arrange
        await using EfDbContext dbContext = CreateDbContext();

        // Act
        bool canConnect = await dbContext.Database.CanConnectAsync();

        // Assert
        Assert.True(canConnect);
    }

    [Fact]
    public async Task Should_Create_And_Query_Database()
    {
        // Arrange
        await using EfDbContext dbContext = CreateDbContext();

        // Act - Ensure database is created
        await dbContext.Database.EnsureCreatedAsync();

        // Assert - Check if we can query the database
        List<WeatherForecast> weatherForecasts = await dbContext.WeatherForecast.ToListAsync();
        Assert.NotNull(weatherForecasts);
    }
}