using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Results;
using CleanArchitectureTemplate.Domain.ValueObjects;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests.Mocks;

public class MockWeatherForecastRepository : IWeatherForecastRepository
{
    public Task Add(WeatherForecast entity)
    {
        throw new NotImplementedException();
    }

    public Task AddRange(IEnumerable<WeatherForecast> entities)
    {
        throw new NotImplementedException();
    }

    public Task<Result<IEnumerable<WeatherForecast>>> Find(Func<WeatherForecast, bool> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<Result<WeatherForecast>> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<WeatherForecast>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IEnumerable<WeatherForecast>>> GetAll()
    {
        var data = new List<WeatherForecast>() {
            new WeatherForecast() {
                Date = new DateTime(2022, 08, 18),
                Temperature = new Temperature(12),
                Summary = "Warm"
            },
            new WeatherForecast() {
                Date = new DateTime(2022, 08, 17),
                Temperature = new Temperature(32),
                Summary = "Mild"
            },
            new WeatherForecast() {
                Date = new DateTime(2022, 06, 18),
                Temperature = new Temperature(5),
                Summary = "Warm"
            },
            new WeatherForecast() {
                Date = new DateTime(2021, 08, 18),
                Temperature = new Temperature(92),
                Summary = "Balmy"
            },
            new WeatherForecast() {
                Date = new DateTime(2019, 10, 25),
                Temperature = new Temperature(45),
                Summary = "Warm"
            },
            new WeatherForecast() {
                Date = new DateTime(2002, 05, 18),
                Temperature = new Temperature(25),
                Summary = "Sweltering"
            },
        };

        return await Task.Run(() => data);
    }

    public Task Remove(WeatherForecast entity)
    {
        throw new NotImplementedException();
    }

    public Task RemoveRange(IEnumerable<WeatherForecast> entities)
    {
        throw new NotImplementedException();
    }

    public Task SaveChanges()
    {
        throw new NotImplementedException();
    }

    public Task Update(WeatherForecast entity, WeatherForecast newValues)
    {
        throw new NotImplementedException();
    }
}