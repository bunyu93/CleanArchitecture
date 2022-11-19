using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Domain.ValueObjects;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Update
{
    public record UpdateWeatherForecast(int Id, UpdateModelWeatherForecast WeatherForecast) : IRequest;

    public class UpdateWeatherForecastHandler : IRequestHandler<UpdateWeatherForecast>
    {
        private readonly IUnitOfWork _context;

        public UpdateWeatherForecastHandler(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateWeatherForecast request, CancellationToken cancellationToken)
        {
            var validator = new UpdateWeatherForecastValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
                throw new ValidationException(JsonSerializer.Serialize(result.Errors));

            var entityCurrent = await _context.WeatherForecastRepository.Get(request.Id);
            var entityNew = entityCurrent;

            if (entityCurrent == null)
            {
                throw new NotFoundException(nameof(WeatherForecast), request.Id.ToString());
            }

            entityNew.Id = request.Id;
            entityNew.Date = System.DateTime.Now;
            entityNew.Temperature = new Temperature()
            {
                Celsius = request.WeatherForecast.Temperature.Celsius,
                Fahrenheit = request.WeatherForecast.Temperature.Fahrenheit
            };
            entityNew.Summary = request.WeatherForecast.Summary;

            await _context.WeatherForecastRepository.Update(entityCurrent, entityNew);
            await _context.SaveAsync();

            return Unit.Value;
        }
    }
}