using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Domain.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Update
{
    public record UpdateWeatherForecast(int Id, UpdateModelWeatherForecast WeatherForecast) : IRequest;

    public class UpdateWeatherForecastHandler : IRequestHandler<UpdateWeatherForecast>
    {
        private readonly IWeatherForecastRepository _context;

        public UpdateWeatherForecastHandler(IWeatherForecastRepository context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateWeatherForecast request, CancellationToken cancellationToken)
        {
            var entityCurrent = await _context.Get(request.Id);
            var entityNew = entityCurrent;

            if (entityCurrent == null)
            {
                throw new NotFoundException(nameof(WeatherForecast), request.Id);
            }

            entityNew.Id = request.Id;
            entityNew.Date = System.DateTime.Now;
            entityNew.Temperature = new Temperature()
            {
                Celsius = request.WeatherForecast.Temperature.Celsius,
                Fahrenheit = request.WeatherForecast.Temperature.Fahrenheit
            };
            entityNew.Summary = request.WeatherForecast.Summary;

            await _context.Update(entityCurrent, entityNew);
            await _context.SaveChanges();

            return Unit.Value;
        }
    }
}