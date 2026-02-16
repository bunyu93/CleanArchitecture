using CleanArchitectureTemplate.Domain.Common.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.DataSeed;

public static class HostExtensionDataSeed
{
    private static readonly Action<ILogger, Exception?> LogSeedingFailed =
        LoggerMessage.Define(LogLevel.Critical, new EventId(1, "SeedingFailed"), "Data seeding failed!");

    private static readonly Action<ILogger, Exception?> LogSeedingStarted =
        LoggerMessage.Define(LogLevel.Information, new EventId(2, "SeedingStarted"), "Seeding data...");

    private static readonly Action<ILogger, Exception?> LogSeedingCompleted =
        LoggerMessage.Define(LogLevel.Information, new EventId(3, "SeedingCompleted"), "Seeding data complete.");

    public static IHost SeedData(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var unitOfWork = scope
            .ServiceProvider
            .GetRequiredService<IUnitOfWork>();

        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(nameof(HostExtensionDataSeed));

        try
        {
            LogSeedingStarted(logger, null);
            Task.WaitAll(
                WeatherData.SeedAsync(unitOfWork)
            );
            LogSeedingCompleted(logger, null);
        }
        catch (System.Exception e)
        {
            LogSeedingFailed(logger, e);
            throw;
        }

        return host;
    }
}
