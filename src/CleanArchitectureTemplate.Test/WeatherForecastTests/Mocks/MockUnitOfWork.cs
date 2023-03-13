using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;

namespace CleanArchitectureTemplate.Test.WeatherForecastTests.Mocks;

public class MockUnitOfWork : IUnitOfWork
{
    public MockUnitOfWork(IWeatherForecastRepository weatherForecast)
    {
        WeatherForecastRepository = weatherForecast;
    }

    public IWeatherForecastRepository WeatherForecastRepository { get; private set; }

    public void Commit()
    {
        throw new NotImplementedException();
    }

    public Task CommitAsync()
    {
        throw new NotImplementedException();
    }

    public void Rollback()
    {
        throw new NotImplementedException();
    }

    public Task RollbackAsync()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync()
    {
        throw new NotImplementedException();
    }
}