using CleanArchitectureTemplate.Application.WeatherForecasts;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Results;
using CleanArchitectureTemplate.Domain.ValueObjects;
using CleanArchitectureTemplate.Persistence.Context;
using CleanArchitectureTemplate.Persistence.Repository;
using CleanArchitectureTemplate.Persistence.Settings.Options;
using CleanArchitectureTemplate.Test.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests;

[Collection("Database")]
public class WeatherForecastTest(DockerPostgresTestFixture fixture)
{
    private EfDbContext CreateDbContext()
    {
        DbContextOptions<EfDbContext> options = new DbContextOptionsBuilder<EfDbContext>().Options;
        IOptions<DatabaseOptions> dbOptions =
            Options.Create(new DatabaseOptions { Connection = fixture.ConnectionString });
        return new EfDbContext(options, dbOptions);
    }

    private static (WeatherForecastsService Service, UnitOfWork UnitOfWork) CreateServiceStack(EfDbContext context)
    {
        WeatherForecastRepository repository = new(context);
        UnitOfWork unitOfWork = new(context, repository);
        WeatherForecastsService service = new(unitOfWork);
        return (service, unitOfWork);
    }

    private async Task ResetDatabaseAsync()
    {
        await using EfDbContext context = CreateDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    private async Task SeedWeatherForecastsAsync()
    {
        await using EfDbContext context = CreateDbContext();
        context.WeatherForecast.AddRange(
            new WeatherForecast
            {
                Date = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Temperature = new Temperature(10),
                Summary = "Cool"
            },
            new WeatherForecast
            {
                Date = new DateTime(2024, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                Temperature = new Temperature(30),
                Summary = "Hot"
            },
            new WeatherForecast
            {
                Date = new DateTime(2024, 12, 25, 0, 0, 0, DateTimeKind.Utc),
                Temperature = new Temperature(0),
                Summary = "Freezing"
            }
        );
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetAll_ReturnsAllSeededRecords()
    {
        await ResetDatabaseAsync();
        await SeedWeatherForecastsAsync();

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            Result<IEnumerable<WeatherForecastQueryModel>> result = await service.GetAll();

            Assert.True(result.IsSuccess);
            List<WeatherForecastQueryModel> items = result.Value.ToList();
            Assert.Equal(3, items.Count);
            Assert.Contains(items, x => x.Summary == "Cool");
            Assert.Contains(items, x => x.Summary == "Hot");
            Assert.Contains(items, x => x.Summary == "Freezing");
        }
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyWhenNoData()
    {
        await ResetDatabaseAsync();

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            Result<IEnumerable<WeatherForecastQueryModel>> result = await service.GetAll();

            Assert.True(result.IsSuccess);
            Assert.Empty(result.Value);
        }
    }

    [Fact]
    public async Task GetAllEf_ReturnsAllSeededRecords()
    {
        await ResetDatabaseAsync();
        await SeedWeatherForecastsAsync();

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            Result<IEnumerable<WeatherForecastQueryModel>> result = await service.GetAllEf();

            Assert.True(result.IsSuccess);
            List<WeatherForecastQueryModel> items = result.Value.ToList();
            Assert.Equal(3, items.Count);
            Assert.Contains(items, x => x.Celsius == 10);
            Assert.Contains(items, x => x.Celsius == 30);
            Assert.Contains(items, x => x.Celsius == 0);
        }
    }

    [Fact]
    public async Task GetById_ReturnsCorrectRecord()
    {
        await ResetDatabaseAsync();
        await SeedWeatherForecastsAsync();

        int seededId;
        await using (EfDbContext seedCtx = CreateDbContext())
        {
            seededId = await seedCtx.WeatherForecast
                .Where(w => w.Summary == "Hot")
                .Select(w => w.Id)
                .FirstAsync();
        }

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            Result<WeatherForecastQueryModel> result = await service.GetById(seededId);

            Assert.True(result.IsSuccess);
            Assert.Equal("Hot", result.Value.Summary);
            Assert.Equal(seededId, result.Value.Id);
        }
    }

    [Fact]
    public async Task GetById_ReturnsFailureForNonExistentId()
    {
        await ResetDatabaseAsync();

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            Result<WeatherForecastQueryModel> result = await service.GetById(99999);

            Assert.False(result.IsSuccess);
        }
    }

    [Fact]
    public async Task Create_PersistsNewRecord()
    {
        await ResetDatabaseAsync();

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            WeatherForecastCreateModel payload = new()
            {
                Date = new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                Temperature = new Temperature(20),
                Summary = "Mild"
            };

            Result result = await service.Create(payload);
            Assert.True(result.IsSuccess);
        }

        await using EfDbContext verifyCtx = CreateDbContext();
        List<WeatherForecast> records = await verifyCtx.WeatherForecast.ToListAsync();
        Assert.Single(records);
        Assert.Equal("Mild", records[0].Summary);
        Assert.Equal(20, records[0].Temperature.Celsius);
    }

    [Fact]
    public async Task Update_ModifiesExistingRecord()
    {
        await ResetDatabaseAsync();
        await SeedWeatherForecastsAsync();

        int targetId;
        await using (EfDbContext seedCtx = CreateDbContext())
        {
            targetId = await seedCtx.WeatherForecast
                .Where(w => w.Summary == "Cool")
                .Select(w => w.Id)
                .FirstAsync();
        }

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            WeatherForecastUpdateModel payload = new()
            {
                Id = targetId, Temperature = new Temperature(99), Summary = "Scorching"
            };

            Result result = await service.Update(payload);
            Assert.True(result.IsSuccess);
        }

        await using EfDbContext verifyCtx = CreateDbContext();
        WeatherForecast? updated = await verifyCtx.WeatherForecast.FindAsync(targetId);
        Assert.NotNull(updated);
        Assert.Equal("Scorching", updated.Summary);
        Assert.Equal(99, updated.Temperature.Celsius);
    }

    [Fact]
    public async Task Update_ReturnsFailureForNonExistentId()
    {
        await ResetDatabaseAsync();

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            WeatherForecastUpdateModel payload = new()
            {
                Id = 99999, Temperature = new Temperature(10), Summary = "Ghost"
            };

            Result result = await service.Update(payload);
            Assert.False(result.IsSuccess);
        }
    }

    [Fact]
    public async Task Delete_RemovesExistingRecord()
    {
        await ResetDatabaseAsync();
        await SeedWeatherForecastsAsync();

        int targetId;
        await using (EfDbContext seedCtx = CreateDbContext())
        {
            targetId = await seedCtx.WeatherForecast
                .Where(w => w.Summary == "Freezing")
                .Select(w => w.Id)
                .FirstAsync();
        }

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            Result result = await service.Delete(targetId);
            Assert.True(result.IsSuccess);
        }

        await using EfDbContext verifyCtx = CreateDbContext();
        List<WeatherForecast> remaining = await verifyCtx.WeatherForecast.ToListAsync();
        Assert.Equal(2, remaining.Count);
        Assert.DoesNotContain(remaining, x => x.Summary == "Freezing");
    }

    [Fact]
    public async Task Delete_ReturnsFailureForNonExistentId()
    {
        await ResetDatabaseAsync();

        await using EfDbContext serviceCtx = CreateDbContext();
        (WeatherForecastsService service, UnitOfWork unitOfWork) = CreateServiceStack(serviceCtx);
        using (unitOfWork)
        {
            Result result = await service.Delete(99999);
            Assert.False(result.IsSuccess);
        }
    }
}