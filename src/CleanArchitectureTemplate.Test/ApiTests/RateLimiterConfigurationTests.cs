using System.Net;
using System.Threading.RateLimiting;
using CleanArchitectureTemplate.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CleanArchitectureTemplate.Test.ApiTests;

public class RateLimiterConfigurationTests
{
    [Fact]
    public async Task AddApiServices_ExemptsHealthEndpointFromGlobalRateLimit()
    {
        using ServiceProvider provider = new ServiceCollection()
            .AddApiServices()
            .BuildServiceProvider();
        RateLimiterOptions options = provider.GetRequiredService<IOptions<RateLimiterOptions>>().Value;
        Assert.NotNull(options.GlobalLimiter);

        DefaultHttpContext context = new();
        context.Request.Path = "/health";
        context.Connection.RemoteIpAddress = IPAddress.Loopback;

        using RateLimitLease lease = await options.GlobalLimiter.AcquireAsync(context, 101);

        Assert.True(lease.IsAcquired);
    }
}
