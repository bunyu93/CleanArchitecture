using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.Repo
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(int id);

        Task<IEnumerable<TEntity>> GetAll();

        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

        Task Add(TEntity entity);

        Task AddRange(IEnumerable<TEntity> entities);

        Task Update(TEntity entity, TEntity newValues);

        Task Remove(TEntity entity);

        Task RemoveRange(IEnumerable<TEntity> entities);

        Task SaveChanges();
    }
}