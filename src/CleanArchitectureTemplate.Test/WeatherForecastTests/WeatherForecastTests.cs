using CleanArchitectureTemplate.Application.WeatherForecasts;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Test.WeatherForecastTests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests;

public class WeatherForecastTests
{
    private IServiceProvider _services;

    public WeatherForecastTests()
    {
        var services = new ServiceCollection();

        services.AddScoped<IWeatherForecastRepository, MockWeatherForecastRepository>();
        services.AddScoped<IUnitOfWork, MockUnitOfWork>();
        services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();

        _services = services.BuildServiceProvider();
    }

    [Fact]
    public async Task WeatherForecast_GetAll_ReturnsAllRecords()
    {
        var weatherForecastService = _services.GetRequiredService<IWeatherForecastsService>();

        var testData = await weatherForecastService.GetAll();

        Assert.Equal(6, testData.Count());
        Assert.Equal("Warm", testData.FirstOrDefault()?.Summary);
        Assert.IsAssignableFrom<IEnumerable<WeatherForecastQueryModel>>(testData);
    }
}