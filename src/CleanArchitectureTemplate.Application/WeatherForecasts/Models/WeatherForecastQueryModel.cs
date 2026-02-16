using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Models;

public class WeatherForecastQueryModel
{
    [Column("id", TypeName = "Integer")] public int Id { get; set; }

    [Column("date", TypeName = "Date")] public DateTime Date { get; set; }

    [Column("fahrenheit", TypeName = "Integer")]
    public int Fahrenheit { get; set; }

    [Column("celsius", TypeName = "Integer")]
    public int Celsius { get; set; }

    [Column("summary", TypeName = "Text")] public string? Summary { get; set; }
}