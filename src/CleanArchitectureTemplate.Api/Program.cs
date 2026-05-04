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

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApiServices();

var app = builder.Build();

// Tasks that needs to be done when WebApplication is build
if (app.Configuration.GetValue<bool>("Database:RunMigrationsOnStartup"))
{
    app.MigrateDb();
}

if (app.Configuration.GetValue<bool>("Database:RunSeedOnStartup"))
{
    app.SeedData();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseRateLimiter();

app.UseHealthChecks("/health");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
