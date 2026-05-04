using CleanArchitectureTemplate.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitectureTemplate.Test.PersistenceTests;

public class ConfigureServicesTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void AddPersistenceServices_WhenConnectionMissing_ThrowsInvalidOperationException(string? connection)
    {
        Dictionary<string, string?> settings = new()
        {
            ["Database:Connection"] = connection
        };
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
            new ServiceCollection().AddPersistenceServices(configuration));

        Assert.Equal("Database:Connection configuration is required.", exception.Message);
    }
}
