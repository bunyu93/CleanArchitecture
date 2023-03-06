using System;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Models
{
    public class WeatherForecastQueryModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public int Fahrenheit { get; set; }

        public int Celsius { get; set; }

        public string? Summary { get; set; }
    }
}