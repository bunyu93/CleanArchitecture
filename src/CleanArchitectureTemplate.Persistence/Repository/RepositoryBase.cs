using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Results;
using CleanArchitectureTemplate.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.Repository;

public class RepositoryBase<TEntity>(EfDbContext context) : IRepository<TEntity> where TEntity : class
{
    private readonly EfDbContext _context = context;

    public async Task<Result<TEntity>> GetById(int id)
    {
        var result = await _context.Set<TEntity>().FindAsync(id);

        if (result is null)
            return Result<TEntity>.Failure(ResultError.NotFound("404", $"Entity with id {id} not found"));
        else
            return Result<TEntity>.Success(result);
    }

    public async Task<Result<TEntity>> GetById(Guid id)
    {
        var result = await _context.Set<TEntity>().FindAsync(id);

        if (result is null)
            return Result<TEntity>.Failure(ResultError.NotFound("404", $"Entity with id {id} not found"));
        else
            return Result<TEntity>.Success(result);
    }

    public Task<Result<IEnumerable<TEntity>>> Find(Func<TEntity, bool> predicate)
    {
        var result = _context.Set<TEntity>().AsEnumerable().Where(predicate);

        if (result is null)
            return Task.FromResult(Result<IEnumerable<TEntity>>.Failure(ResultError.Failure("500", "Cannot get the entities")));
        else
            return Task.FromResult(Result<IEnumerable<TEntity>>.Success(result));
    }

    public async Task<Result<IEnumerable<TEntity>>> GetAll()
    {
        var result = await _context.Set<TEntity>().ToListAsync();

        if (result is null)
            return Result<IEnumerable<TEntity>>.Failure(ResultError.Failure("500", "Cannot get the entities"));
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

    public Task Update(TEntity entity, TEntity newValues)
    {
        _context.Entry(entity).CurrentValues.SetValues(newValues);
        return Task.CompletedTask;
    }

    public Task Remove(TEntity entity)
    {
        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task RemoveRange(IEnumerable<TEntity> entities)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}
