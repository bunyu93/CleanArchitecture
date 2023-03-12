using CleanArchitectureTemplate.Domain.Common.Database;
using System.Data;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests;

public class MockDapperDbContext : IDapperContext
{
    public IDbConnection CreateConnection()
    {
        throw new NotImplementedException();
    }

    public Task<TResult> QueryFirstOrDefaultHandler<TResult>(string query)
    {
        throw new NotImplementedException();
    }

    public Task<TResult> QueryFirstOrDefaultHandler<TResult>(string query, object param)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResult>> QueryHandler<TResult>(string query)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TResult>> QueryHandler<TResult>(string query, object param)
    {
        throw new NotImplementedException();
    }
}