using CleanArchitectureTemplate.Api;
using CleanArchitectureTemplate.Api.Middlewares;
using CleanArchitectureTemplate.Application;
using CleanArchitectureTemplate.Application.DataSeed;
using CleanArchitectureTemplate.Infrastructure;
using CleanArchitectureTemplate.Persistence;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 1_048_576;
});

builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

if (app.Configuration.GetValue<bool>("Database:RunMigrationsOnStartup"))
{
    app.MigrateDb();
}

if (app.Configuration.GetValue<bool>("Database:RunSeedOnStartup"))
{
    app.SeedData();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseHsts();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<SecurityHeadersMiddleware>();

app.UseHttpsRedirection();
app.UseRateLimiter();

app.UseHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
