using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using System.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Domain.Common.Database;

public interface IUnitOfWork
{
    IWeatherForecastRepository WeatherForecastRepository { get; }

    Task<IQueryable<T>> SqlQuery<T>(FormattableString sql);

    void Save();

    void Commit();

    void Rollback();

    Task SaveAsync(CancellationToken cancellationToken = default);

    Task CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}
