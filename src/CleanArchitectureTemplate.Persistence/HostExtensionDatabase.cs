﻿using CleanArchitectureTemplate.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureTemplate.Persistence
{
    public static class HostExtensionDatabase
    {
        public static IHost MigrateDbAndSeedData(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var dbContext = scope
                .ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(nameof(HostExtensionDatabase));

            dbContext.Database.EnsureCreated();

            try
            {
                dbContext.Database.Migrate();
                DataSeeder.Seed(dbContext);
            }
            catch (System.Exception e)
            {
                logger.LogCritical(e, "Migration or seed failed!");
            }

            return host;
        }
    }
}