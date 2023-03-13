using CleanArchitectureTemplate.Application.WeatherForecasts;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Test.WeatherForecastTests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests;

public class WeatherForecastTest
{
    private IServiceProvider _services;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();

        services.AddScoped<IWeatherForecastRepository, MockWeatherForecastRepository>();
        services.AddScoped<IDapperContext, MockDapperDbContext>();
        services.AddScoped<IUnitOfWork, MockUnitOfWork>();
        services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();

        _services = services.BuildServiceProvider();
    }

    [Test]
    public async Task WeatherForecast_GetAll_ReturnsAllRecords()
    {
        var weatherForecastService = _services.GetRequiredService<IWeatherForecastsService>();

        var testData = await weatherForecastService.GetAll();

        Assert.That(testData.Count(), Is.EqualTo(6));
        Assert.That(testData, Is.InstanceOf<IEnumerable<WeatherForecastQueryModel>>());
        Assert.That(testData.FirstOrDefault()?.Summary, Is.EqualTo("Warm"));
    }
}