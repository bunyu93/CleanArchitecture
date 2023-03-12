using CleanArchitectureTemplate.Application.WeatherForecasts;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Persistence.EntityFramework;
using CleanArchitectureTemplate.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Test.WeatherForecast;

public class WeatherForecastTest
{
    private IServiceProvider _services;

    [SetUp]
    public async Task Setup()
    {
        var services = new ServiceCollection();

        services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();

        // TODO: Need to mock results
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

        services.AddSingleton<IDapperContext, DapperDbContext>();

        _services = await Task.Run(() => services.BuildServiceProvider());
    }

    [Test]
    public async Task WeatherForecast_GetAll()
    {
        var weatherForecastService = _services.GetRequiredService<WeatherForecastsService>();

        var testData = await weatherForecastService.GetAll();

        Assert.That(testData.Any(), Is.True);
    }
}