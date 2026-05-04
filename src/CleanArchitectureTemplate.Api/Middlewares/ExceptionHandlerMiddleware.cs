using CleanArchitectureTemplate.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace CleanArchitectureTemplate.Api.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    private static readonly JsonSerializerOptions ProblemDetailsJsonOptions = new(JsonSerializerDefaults.Web);

    private static readonly Action<ILogger, Exception> LogUnhandled =
        LoggerMessage.Define(LogLevel.Error, new EventId(1, "UnhandledException"), "Unhandled exception while processing request");

    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            LogUnhandled(_logger, error);

            if (context.Response.HasStarted)
            {
                throw;
            }

            var statusCode = error switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,
            };

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Status = statusCode,
                Title = ReasonPhrases(statusCode),
                Detail = statusCode == (int)HttpStatusCode.InternalServerError ? null : error.Message,
                Instance = context.Request.Path,
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, ProblemDetailsJsonOptions));
        }
    }

    private static string ReasonPhrases(int statusCode) => statusCode switch
    {
        400 => "Bad Request",
        404 => "Not Found",
        _ => "Internal Server Error",
    };
}
