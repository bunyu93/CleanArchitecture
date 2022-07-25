using System;

namespace CleanArchitectureTemplate.Domain.Exceptions
{
    public class UnsupportedWeatherForecastException : Exception
    {
        public UnsupportedWeatherForecastException(string code)
            : base($"WeatherForecast \"{code}\" is unsupported.")
        {
        }
    }
}