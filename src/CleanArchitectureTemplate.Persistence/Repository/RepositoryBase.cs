using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Result;
using CleanArchitectureTemplate.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.Repository;

public class RepositoryBase<TEntity>(EfDbContext context) : IRepository<TEntity> where TEntity : class
{
    protected readonly EfDbContext _context = context;

    public async Task<Result<TEntity>> Get(int id)
    {
        var result = await _context.Set<TEntity>().FindAsync(id);

        if (result is null)
            return Result<TEntity>.Failure(Error.NotFound("404", $"Entity with id {id} not found"));
        else
            return Result<TEntity>.Success(result);
    }

    public async Task<Result<TEntity>> Get(Guid id)
    {
        var result = await _context.Set<TEntity>().FindAsync(id);

        if (result is null)
            return Result<TEntity>.Failure(Error.NotFound("404", $"Entity with id {id} not found"));
        else
            return Result<TEntity>.Success(result);
    }

    public async Task<Result<IEnumerable<TEntity>>> Find(Func<TEntity, bool> predicate)
    {
        var result = await Task.Run(() => _context.Set<TEntity>().Where(predicate));

        if (result is null)
            return Result<IEnumerable<TEntity>>.Failure(Error.Failure("500", "Cannot get the entities"));
        else
            return Result<IEnumerable<TEntity>>.Success(result);
    }

    public async Task<Result<IEnumerable<TEntity>>> GetAll()
    {
        var result = await _context.Set<TEntity>().ToListAsync();

        if (result is null)
            return Result<IEnumerable<TEntity>>.Failure(Error.Failure("500", "Cannot get the entities"));
        else
            return Result<IEnumerable<TEntity>>.Success(result);
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