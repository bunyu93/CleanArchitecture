using CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Update;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Domain.ValueObjects;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Create
{
    public record CreateWeatherForecast(CreateModelWeatherForecast WeatherForecast) : IRequest;

    public class CreateWeatherForecastHandler : IRequestHandler<CreateWeatherForecast>
    {
        private readonly IUnitOfWork _context;

        public CreateWeatherForecastHandler(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CreateWeatherForecast request, CancellationToken cancellationToken)
        {
            var validator = new CreateWeatherForecastValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
                throw new ValidationException(JsonSerializer.Serialize(result.Errors));

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

            await _context.WeatherForecastRepository.Add(entity);
            await _context.SaveAsync();

            return Unit.Value;
        }
    }
}