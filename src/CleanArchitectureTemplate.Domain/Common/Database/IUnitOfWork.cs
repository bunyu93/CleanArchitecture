using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Domain.Common.Database
{
    public interface IUnitOfWork
    {
        IWeatherForecastRepository WeatherForecastRepository { get; }

        void Save();

        void Commit();

        void Rollback();

        Task SaveAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}