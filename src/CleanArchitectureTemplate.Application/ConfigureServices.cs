using CleanArchitectureTemplate.Application.WeatherForecasts;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherForecastsService, WeatherForecastsService>();

        return services;
    }
}