using CleanArchitectureTemplate.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace CleanArchitectureTemplate.Api.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new { error.Message };

            response.StatusCode = error switch
            {
                NotFoundException => (int)HttpStatusCode.NotFound,
                ValidationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError,// unhandled error
            };

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);
            throw;
        }
    }
}