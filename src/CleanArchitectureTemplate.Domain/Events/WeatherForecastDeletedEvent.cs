﻿using CleanArchitectureTemplate.Domain.Common;
using CleanArchitectureTemplate.Domain.Entities;

namespace CleanArchitectureTemplate.Domain.Events
{
    public class WeatherForecastDeletedEvent : BaseEvent
    {
        public WeatherForecastDeletedEvent(WeatherForecast weather)
        {
            Weather = weather;
        }

        public WeatherForecast Weather { get; }
    }
}