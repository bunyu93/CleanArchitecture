﻿using System;

namespace CleanArchitectureTemplate.Domain.Entities;

public class WeatherForecast : Entity
{
    public DateTime Date { get; set; }

    public Temperature Temperature { get; set; } = new Temperature();

    public string? Summary { get; set; }
}