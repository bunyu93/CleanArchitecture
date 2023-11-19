using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.ValueObjects;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests.Mocks;

public static class MockWeatherForecastData
{
    public static IEnumerable<WeatherForecast> WeatherForecastAll()
    {
        return new List<WeatherForecast>() {
            new WeatherForecast() {
                Date = new DateTime(2022, 08, 18),
                Temperature = new Temperature(12),
                Summary = "Warm"
            },
            new WeatherForecast() {
                Date = new DateTime(2022, 08, 17),
                Temperature = new Temperature(32),
                Summary = "Mild"
            },
            new WeatherForecast() {
                Date = new DateTime(2022, 06, 18),
                Temperature = new Temperature(5),
                Summary = "Warm"
            },
            new WeatherForecast() {
                Date = new DateTime(2021, 08, 18),
                Temperature = new Temperature(92),
                Summary = "Balmy"
            },
            new WeatherForecast() {
                Date = new DateTime(2019, 10, 25),
                Temperature = new Temperature(45),
                Summary = "Warm"
            },
            new WeatherForecast() {
                Date = new DateTime(2002, 05, 18),
                Temperature = new Temperature(25),
                Summary = "Sweltering"
            },
        };
    }

    public static IEnumerable<WeatherForecastQueryModel> WeatherForecastQueryModelAll()
    {
        return new List<WeatherForecastQueryModel>() {
            new() {
                Date = new DateTime(2022, 08, 18),
                Fahrenheit = 12,
                Celsius = -11,
                Summary = "Warm"
            },
            new() {
                Date = new DateTime(2022, 08, 17),
                Fahrenheit = 32,
                Celsius = 18,
                Summary = "Mild"
            },
            new() {
                Date = new DateTime(2022, 06, 18),
                Fahrenheit = 5,
                Celsius = -15 ,
                Summary = "Warm"
            },
            new() {
                Date = new DateTime(2021, 08, 18),
                Fahrenheit = 92,
                Celsius = 33,
                Summary = "Balmy"
            },
            new() {
                Date = new DateTime(2019, 10, 25),
                Fahrenheit = 45,
                Celsius = 7,
                Summary = "Warm"
            },
            new() {
                Date = new DateTime(2002, 05, 18),
                Fahrenheit = 25,
                Celsius = -4,
                Summary = "Sweltering"
            },
        };
    }
}