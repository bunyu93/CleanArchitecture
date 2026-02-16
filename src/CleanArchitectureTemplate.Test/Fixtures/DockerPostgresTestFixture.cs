using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Testcontainers.PostgreSql;

namespace CleanArchitectureTemplate.Test.Fixtures;

public sealed class DockerPostgresTestFixture : IAsyncLifetime
{
    private static readonly Action<ILogger, string, Exception?> LogDockerPostgresTestFixtureStart =
        LoggerMessage.Define<string>(
            LogLevel.Information,
            new EventId(1, nameof(LogDockerPostgresTestFixtureStart)),
            "PostgreSQL test container started. Connection: {ConnectionString}"
        );

    private static readonly Action<ILogger, Exception?> LogDockerPostgresTestFixtureDispose =
        LoggerMessage.Define(
            LogLevel.Information,
            new EventId(2, nameof(LogDockerPostgresTestFixtureDispose)),
            "PostgreSQL test container stopped and removed");

    private readonly PostgreSqlContainer _container = new PostgreSqlBuilder()
        .WithImage("postgres:17-alpine")
        .WithDatabase("weatherdb_test")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();

    private readonly ILogger<DockerPostgresTestFixture> _logger = NullLogger<DockerPostgresTestFixture>.Instance;

    public string ConnectionString => _container.GetConnectionString();

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        LogDockerPostgresTestFixtureStart(_logger, ConnectionString, null);
    }

    public async Task DisposeAsync()
    {
        await _container.DisposeAsync();
        LogDockerPostgresTestFixtureDispose(_logger, null);
    }
}