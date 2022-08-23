using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Create
{
    public record CreateWeatherForecast(CreateModelWeatherForecast WeatherForecast) : IRequest;

    public class CreateWeatherForecastHandler : IRequestHandler<CreateWeatherForecast>
    {
        private readonly IWeatherForecastRepository _context;

        public CreateWeatherForecastHandler(IWeatherForecastRepository context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateWeatherForecast request, CancellationToken cancellationToken)
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