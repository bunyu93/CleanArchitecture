using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Persistence.EntityFramework;
using System;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Persistence.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private EfDbContext _context;
        private bool _disposed = false;

        public UnitOfWork(EfDbContext context)
        {
            _context = context;
            WeatherForecastRepository = new WeatherForecastRepository(_context);
        }

        public IWeatherForecastRepository WeatherForecastRepository { get; private set; }

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
}