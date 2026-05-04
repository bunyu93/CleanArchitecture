using System.Net;
using System.Text.Json;
using CleanArchitectureTemplate.Api.Middlewares;
using CleanArchitectureTemplate.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CleanArchitectureTemplate.Test.MiddlewareTests;

public class ExceptionHandlerMiddlewareTests
{
    private static readonly JsonSerializerOptions ProblemDetailsJsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    [Fact]
    public async Task Invoke_WhenUnhandledException_ReturnsProblemJsonWithoutInternalDetails()
    {
        const string secretDetail = "database password leaked";
        DefaultHttpContext context = CreateHttpContext();
        ListLogger<ExceptionHandlerMiddleware> logger = new();
        ExceptionHandlerMiddleware middleware = new(_ => throw new InvalidOperationException(secretDetail), logger);

        await middleware.Invoke(context);

        Assert.Equal(StatusCodes.Status500InternalServerError, context.Response.StatusCode);
        Assert.Equal("application/problem+json", context.Response.ContentType);
        (ProblemDetails problem, string responseBody) = await ReadProblemDetails(context);
        AssertProblemDetailsUsesCamelCase(responseBody);
        Assert.Equal(StatusCodes.Status500InternalServerError, problem.Status);
        Assert.Equal("Internal Server Error", problem.Title);
        Assert.Null(problem.Detail);
        Assert.DoesNotContain(secretDetail, responseBody, StringComparison.OrdinalIgnoreCase);
        Assert.Contains(logger.Entries, entry => entry.LogLevel == LogLevel.Error && entry.Exception is InvalidOperationException);
    }

    [Fact]
    public async Task Invoke_WhenKnownException_ReturnsProblemJsonWithSafeDetail()
    {
        DefaultHttpContext context = CreateHttpContext("/missing-resource");
        ListLogger<ExceptionHandlerMiddleware> logger = new();
        ExceptionHandlerMiddleware middleware = new(_ => throw new NotFoundException("Forecast", "123"), logger);

        await middleware.Invoke(context);

        Assert.Equal(StatusCodes.Status404NotFound, context.Response.StatusCode);
        Assert.Equal("application/problem+json", context.Response.ContentType);
        (ProblemDetails problem, string responseBody) = await ReadProblemDetails(context);
        AssertProblemDetailsUsesCamelCase(responseBody);
        Assert.Equal((int)HttpStatusCode.NotFound, problem.Status);
        Assert.Equal("Not Found", problem.Title);
        Assert.Equal("Entity 'Forecast' with id: '123' was not found.", problem.Detail);
        Assert.Equal("/missing-resource", problem.Instance);
    }

    [Fact]
    public async Task Invoke_WhenValidationException_ReturnsBadRequestProblemJson()
    {
        DefaultHttpContext context = CreateHttpContext();
        ListLogger<ExceptionHandlerMiddleware> logger = new();
        ExceptionHandlerMiddleware middleware = new(_ => throw new ValidationException("Invalid summary"), logger);

        await middleware.Invoke(context);

        (ProblemDetails problem, string responseBody) = await ReadProblemDetails(context);
        AssertProblemDetailsUsesCamelCase(responseBody);
        Assert.Equal(StatusCodes.Status400BadRequest, context.Response.StatusCode);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.Status);
        Assert.Equal("Invalid summary", problem.Detail);
    }

    private static DefaultHttpContext CreateHttpContext(string path = "/weather-forecast")
    {
        DefaultHttpContext context = new();
        context.Request.Path = path;
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<(ProblemDetails Problem, string ResponseBody)> ReadProblemDetails(HttpContext context)
    {
        context.Response.Body.Position = 0;
        using StreamReader reader = new(context.Response.Body, leaveOpen: true);
        string responseBody = await reader.ReadToEndAsync();
        ProblemDetails? problem = JsonSerializer.Deserialize<ProblemDetails>(responseBody, ProblemDetailsJsonOptions);

        Assert.NotNull(problem);
        return (problem, responseBody);
    }

    private static void AssertProblemDetailsUsesCamelCase(string responseBody)
    {
        using JsonDocument document = JsonDocument.Parse(responseBody);
        JsonElement root = document.RootElement;
        Assert.True(root.TryGetProperty("status", out _));
        Assert.True(root.TryGetProperty("title", out _));
        Assert.True(root.TryGetProperty("instance", out _));
        Assert.False(root.TryGetProperty("Status", out _));
        Assert.False(root.TryGetProperty("Title", out _));
        Assert.False(root.TryGetProperty("Detail", out _));
        Assert.False(root.TryGetProperty("Instance", out _));
    }

    private sealed class ListLogger<T> : ILogger<T>
    {
        public List<LogEntry> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            Entries.Add(new LogEntry(logLevel, exception));
        }
    }

    private sealed record LogEntry(LogLevel LogLevel, Exception? Exception);
}
