﻿using CleanArchitectureTemplate.Domain.Common;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Persistence.EntityFramework;
using CleanArchitectureTemplate.Persistence.Repo;
using CleanArchitectureTemplate.Persistence.Settings.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Persistence
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.Database));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetValue<string>("Database:Connection")));

            services.AddScoped<IRepository<WeatherForecast>, Repository<WeatherForecast>>();
            services.AddSingleton<IDapperContext, DapperContext>();

            return services;
        }
    }
}