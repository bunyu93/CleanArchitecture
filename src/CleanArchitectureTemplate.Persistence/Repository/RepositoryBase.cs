using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.Repository
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly EfDbContext _context;

        public RepositoryBase(EfDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> Get(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id) ?? throw new NotFoundException($"Entity with id {id} not found");
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => _context.Set<TEntity>().Where(predicate));
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task Add(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public async Task Update(TEntity entity, TEntity newValues)
        {
            await Task.Run(() => _context.Entry(entity).CurrentValues.SetValues(newValues));
        }

        public async Task Remove(TEntity entity)
        {
            await Task.Run(() => _context.Set<TEntity>().Remove(entity));
        }

        public async Task RemoveRange(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => _context.Set<TEntity>().RemoveRange(entities));
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}