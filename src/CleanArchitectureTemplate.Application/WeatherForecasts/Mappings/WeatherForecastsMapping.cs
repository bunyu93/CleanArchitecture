using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Mappings;

public static class WeatherForecastsMapping
{
    public static WeatherForecastQueryModel MapToQueryModel(this WeatherForecast payload)
    {
        return new WeatherForecastQueryModel()
        {
            Id = payload.Id,
            Celsius = payload.Temperature.Celsius,
            Fahrenheit = payload.Temperature.Fahrenheit,
            Summary = payload.Summary,
            Date = payload.Date,
        };
    }

    public static WeatherForecast MapToWeatherForecast(this WeatherForecastCreateModel payload)
    {
        return new WeatherForecast()
        {
            Id = payload.Id,
            Temperature = payload.Temperature,
            Summary = payload.Summary,
            Date = payload.Date,
        };
    }

    public static WeatherForecast MapToWeatherForecast(this WeatherForecastUpdateModel payload)
    {
        return new WeatherForecast()
        {
            Id = payload.Id,
            Temperature = payload.Temperature,
            Summary = payload.Summary,
            Date = payload.Date,
        };
    }
}