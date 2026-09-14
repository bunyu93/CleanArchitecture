using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Results;
using CleanArchitectureTemplate.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.Repository;

public class RepositoryBase<TEntity>(EfDbContext context) : IRepository<TEntity> where TEntity : class
{
    private readonly EfDbContext _context = context;

    public async Task<Result<TEntity>> GetById(int id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken);

        if (result is null)
            return Result.Failure<TEntity>(ResultError.NotFound("Entity.NotFound", $"Entity with id {id} not found"));
        else
            return Result.Success(result);
    }

    public async Task<Result<TEntity>> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _context.Set<TEntity>().FindAsync(new object?[] { id }, cancellationToken);

        if (result is null)
            return Result.Failure<TEntity>(ResultError.NotFound("Entity.NotFound", $"Entity with id {id} not found"));
        else
            return Result.Success(result);
    }

    public Task<Result<IEnumerable<TEntity>>> Find(Func<TEntity, bool> predicate, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = _context.Set<TEntity>().AsEnumerable().Where(predicate);

        if (result is null)
            return Task.FromResult(Result.Failure<IEnumerable<TEntity>>(ResultError.Failure("Entity.Failure", "Cannot get the entities")));
        else
            return Task.FromResult(Result.Success<IEnumerable<TEntity>>(result));
    }

    public async Task<Result<IEnumerable<TEntity>>> GetAll(CancellationToken cancellationToken = default)
    {
        var result = await _context.Set<TEntity>().ToListAsync(cancellationToken);

        if (result is null)
            return Result.Failure<IEnumerable<TEntity>>(ResultError.Failure("Entity.Failure", "Cannot get the entities"));
        else
            return Result.Success<IEnumerable<TEntity>>(result);
    }

    public async Task Add(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddAsync(entity, cancellationToken);
    }

    public async Task AddRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
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

    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
