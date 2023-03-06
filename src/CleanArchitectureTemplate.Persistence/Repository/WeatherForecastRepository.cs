using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Persistence.EntityFramework;

namespace CleanArchitectureTemplate.Persistence.Repository
{
    public class WeatherForecastRepository : RepositoryBase<WeatherForecast>, IWeatherForecastRepository
    {
        public WeatherForecastRepository(EfDbContext context) : base(context)
        { }
    }
}