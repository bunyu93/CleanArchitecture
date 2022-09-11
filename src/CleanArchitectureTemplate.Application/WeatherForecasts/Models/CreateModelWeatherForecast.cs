using CleanArchitectureTemplate.Domain.ValueObjects;
using System;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Models
{
    public class CreateModelWeatherForecast
    {
        public DateTime Date { get; set; }

        public Temperature Temperature { get; set; } = new Temperature();

        public string? Summary { get; set; }
    }
}