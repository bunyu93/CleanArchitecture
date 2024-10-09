using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.Repository;

public class UnitOfWork(EfDbContext context, IWeatherForecastRepository weatherForecastRepository) : IUnitOfWork, IDisposable
{
    private EfDbContext _context = context;
    private bool _disposed = false;

    public IWeatherForecastRepository WeatherForecastRepository { get; private set; } = weatherForecastRepository;

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Commit()
    {
        _context.Database.CommitTransaction();
    }

    public void Rollback()
    {
        _context.Database.RollbackTransaction();
    }

    public async Task<IQueryable<T>> SqlQueryRaw<T>(string sql, params object[] parameters)
    {
        return await Task.Run(() => _context.Database.SqlQueryRaw<T>(sql, parameters));
    }

    public async Task<IQueryable<T>> SqlQuery<T>(FormattableString sql)
    {
        return await Task.Run(() => _context.Database.SqlQuery<T>(sql));
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task CommitAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }

    public async Task RollbackAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}