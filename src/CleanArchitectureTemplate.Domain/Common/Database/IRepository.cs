using CleanArchitectureTemplate.Domain.Results;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Domain.Common.Database;

public interface IRepository<TEntity> where TEntity : class
{
    Task<Result<TEntity>> GetById(int id, CancellationToken cancellationToken = default);

    Task<Result<TEntity>> GetById(Guid id, CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<TEntity>>> GetAll(CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<TEntity>>> Find(Func<TEntity, bool> predicate, CancellationToken cancellationToken = default);

    Task Add(TEntity entity, CancellationToken cancellationToken = default);

    Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

    Task Update(TEntity entity, TEntity newValues);

    Task Remove(TEntity entity);

    Task RemoveRange(IEnumerable<TEntity> entities);

    Task SaveChanges(CancellationToken cancellationToken = default);
}