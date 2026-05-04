using CleanArchitectureTemplate.Api.Middlewares;
using Microsoft.AspNetCore.Http;

namespace CleanArchitectureTemplate.Test.MiddlewareTests;

public class SecurityHeadersMiddlewareTests
{
    [Fact]
    public async Task Invoke_AddsBaselineSecurityHeaders()
    {
        DefaultHttpContext context = new();
        context.Response.Body = new MemoryStream();
        SecurityHeadersMiddleware middleware = new(ctx => ctx.Response.WriteAsync(string.Empty));

        await middleware.Invoke(context);

        Assert.Equal("nosniff", context.Response.Headers["X-Content-Type-Options"]);
        Assert.Equal("DENY", context.Response.Headers["X-Frame-Options"]);
        Assert.Equal("no-referrer", context.Response.Headers["Referrer-Policy"]);
        Assert.Equal("camera=(), microphone=(), geolocation=()", context.Response.Headers["Permissions-Policy"]);
        Assert.Equal("none", context.Response.Headers["X-Permitted-Cross-Domain-Policies"]);
    }
}
