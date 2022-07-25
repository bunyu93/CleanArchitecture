using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.ValueObjects;
using CleanArchitectureTemplate.Persistence.Repo;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Create
{
    public record CreateWeatherForecasts(CreateModelWeatherForecasts WeatherForecast) : IRequest;

    public class CreateWeatherForecastsHandler : IRequestHandler<CreateWeatherForecasts>
    {
        private readonly IRepository<WeatherForecast> _context;

        public CreateWeatherForecastsHandler(IRepository<WeatherForecast> context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateWeatherForecasts request, CancellationToken cancellationToken)
        {
            var entity = new WeatherForecast()
            {
                Date = request.WeatherForecast.Date,
                Temperature = new Temperature()
                {
                    Celsius = request.WeatherForecast.Temperature.Celsius,
                    Fahrenheit = request.WeatherForecast.Temperature.Fahrenheit
                },
                Summary = request.WeatherForecast.Summary
            };

            await _context.Add(entity);
            await _context.SaveChanges();

            return Unit.Value;
        }
    }
}