using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Domain.Common.Database;

public interface IUnitOfWork
{
    IWeatherForecastRepository WeatherForecastRepository { get; }

    Task<IQueryable<T>> SqlQueryRaw<T>(string sql, params object[] parameters);

    Task<IQueryable<T>> SqlQuery<T>(FormattableString sql);

    void Save();

    void Commit();

    void Rollback();

    Task SaveAsync();

    Task CommitAsync();

    Task RollbackAsync();
}