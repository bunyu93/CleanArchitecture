using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using System.Data;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests.Mocks;

public class MockDapperDbContext : IDapperContext
{
    public IDbConnection CreateConnection()
    {
        throw new NotImplementedException();
    }

    public async Task<TResult> QueryFirstOrDefaultHandler<TResult>(string query)
    {
        var data = MockWeatherForecastData.WeatherForecastAll();
        var list = new List<WeatherForecastQueryModel>();

        foreach (var x in data)
        {
            list.Add(new WeatherForecastQueryModel()
            {
                Id = x.Id,
                Celsius = x.Temperature.Celsius,
                Fahrenheit = x.Temperature.Fahrenheit,
                Date = x.Date,
                Summary = x.Summary,
            });
        }

        return await Task.Run(() => ((IEnumerable<TResult>)list).FirstOrDefault() ?? throw new Exception());
    }

    public Task<TResult> QueryFirstOrDefaultHandler<TResult>(string query, object param)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<TResult>> QueryHandler<TResult>(string query)
    {
        var data = MockWeatherForecastData.WeatherForecastAll();
        var list = new List<WeatherForecastQueryModel>();

        foreach (var x in data)
        {
            list.Add(new WeatherForecastQueryModel()
            {
                Id = x.Id,
                Celsius = x.Temperature.Celsius,
                Fahrenheit = x.Temperature.Fahrenheit,
                Date = x.Date,
                Summary = x.Summary,
            });
        }

        return await Task.Run(() => (IEnumerable<TResult>)list);
    }

    public Task<IEnumerable<TResult>> QueryHandler<TResult>(string query, object param)
    {
        throw new NotImplementedException();
    }
}