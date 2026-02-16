using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Persistence.Context;
using CleanArchitectureTemplate.Persistence.Repository;
using CleanArchitectureTemplate.Persistence.Settings.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Persistence;

public static class ConfigureServices
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Database));

        services.AddDbContext<EfDbContext>(options =>
            options.UseNpgsql(configuration.GetValue<string>("Database:Connection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IWeatherForecastRepository, WeatherForecastRepository>();

        return services;
    }
}