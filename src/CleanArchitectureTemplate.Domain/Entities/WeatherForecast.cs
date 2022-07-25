using System;

namespace CleanArchitectureTemplate.Domain.Entities
{
    public class WeatherForecast : BaseEntity
    {
        public DateTime Date { get; set; }

        public Temperature Temperature { get; set; }

        public string? Summary { get; set; }
    }
}