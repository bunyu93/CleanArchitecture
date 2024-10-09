using CleanArchitectureTemplate.Domain.Result;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Domain.Common.Database;

public interface IRepository<TEntity> where TEntity : class
{
    Task<Result<TEntity>> Get(int id);

    Task<Result<TEntity>> Get(Guid id);

    Task<Result<IEnumerable<TEntity>>> GetAll();

    Task<Result<IEnumerable<TEntity>>> Find(Func<TEntity, bool> predicate);

    Task Add(TEntity entity);

    Task AddRange(IEnumerable<TEntity> entities);

    Task Update(TEntity entity, TEntity newValues);

    Task Remove(TEntity entity);

    Task RemoveRange(IEnumerable<TEntity> entities);

    Task SaveChanges();
}