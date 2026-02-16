using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application;

public static class WeatherData
{
    public static async Task SeedAsync(IUnitOfWork unitOfWork)
    {
        await WeatherDataAsync(unitOfWork);
    }

    private static async Task WeatherDataAsync(IUnitOfWork unitOfWork)
    {
        var weatherForecastRepository = unitOfWork.WeatherForecastRepository;

        var existingData = await weatherForecastRepository.GetAll();
        if (existingData.Value.Any())
        {
            return; // DB has been seeded
        }

        List<WeatherForecast> list = new()
        {
            new WeatherForecast()
            {
                Date = new DateTime(2022, 08, 18),
                Temperature = new Temperature(12),
                Summary = "Warm"
            },
            new WeatherForecast()
            {
                Date = new DateTime(2022, 08, 17),
                Temperature = new Temperature(32),
                Summary = "Mild"
            },
            new WeatherForecast()
            {
                Date = new DateTime(2022, 06, 18),
                Temperature = new Temperature(5),
                Summary = "Warm"
            },
            new WeatherForecast()
            {
                Date = new DateTime(2021, 08, 18),
                Temperature = new Temperature(92),
                Summary = "Balmy"
            },
            new WeatherForecast()
            {
                Date = new DateTime(2019, 10, 25),
                Temperature = new Temperature(45),
                Summary = "Warm"
            },
            new WeatherForecast()
            {
                Date = new DateTime(2002, 05, 18),
                Temperature = new Temperature(25),
                Summary = "Sweltering"
            },
        };

        foreach (var item in list)
        {
            await weatherForecastRepository.Add(item);
        }

        await unitOfWork.SaveAsync();
    }    
}
