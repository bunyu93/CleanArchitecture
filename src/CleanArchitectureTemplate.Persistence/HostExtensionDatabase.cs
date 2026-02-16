using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace CleanArchitectureTemplate.Persistence;

public static class HostExtensionDatabase
{
    private static readonly Action<ILogger, Exception> LogMigrationFailed =
        LoggerMessage.Define(LogLevel.Critical, new EventId(1, "MigrationFailed"), "Migration or seed failed!");
    public static IHost MigrateDb(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        using var dbContext = scope
            .ServiceProvider
            .GetRequiredService<EfDbContext>();

        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(nameof(HostExtensionDatabase));

        try
        {
            dbContext.Database.Migrate();
        }
        catch (System.Exception e)
        {
            // Optionally, you can rethrow the exception if you want to stop the application startup
            LogMigrationFailed(logger, e);
            throw;
        }

        return host;
    }
}