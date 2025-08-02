using System;

namespace CleanArchitectureTemplate.Domain.Exceptions;

public class UnsupportedWeatherForecastException : Exception
{
    public UnsupportedWeatherForecastException()
    {
    }
    public UnsupportedWeatherForecastException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public UnsupportedWeatherForecastException(string code)
        : base($"WeatherForecast \"{code}\" is unsupported.")
    {
    }
}