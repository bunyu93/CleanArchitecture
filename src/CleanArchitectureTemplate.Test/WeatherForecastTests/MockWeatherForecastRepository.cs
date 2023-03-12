using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.ValueObjects;
using System.Linq.Expressions;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests;

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

    public Task<IEnumerable<WeatherForecast>> Find(Expression<Func<WeatherForecast, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<WeatherForecast> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task<WeatherForecast> Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<WeatherForecast>> GetAll()
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