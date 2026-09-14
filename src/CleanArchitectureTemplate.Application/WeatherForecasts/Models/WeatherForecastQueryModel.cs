using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Models;

public class WeatherForecastQueryModel
{
    [Column("id")] public int Id { get; set; }

    [Column("date")] public DateTime Date { get; set; }

    [Column("fahrenheit")] public int Fahrenheit { get; set; }

    [Column("celsius")] public int Celsius { get; set; }

    [Column("summary")] public string? Summary { get; set; }
}
