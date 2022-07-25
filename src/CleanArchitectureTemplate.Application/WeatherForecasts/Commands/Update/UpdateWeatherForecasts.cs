using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Domain.ValueObjects;
using CleanArchitectureTemplate.Persistence.Repo;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Update
{
    public record UpdateWeatherForecasts(int Id, UpdateModelWeatherForecasts WeatherForecast) : IRequest;

    public class UpdateWeatherForecastsHandler : IRequestHandler<UpdateWeatherForecasts>
    {
        private readonly IRepository<WeatherForecast> _context;

        public UpdateWeatherForecastsHandler(IRepository<WeatherForecast> context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateWeatherForecasts request, CancellationToken cancellationToken)
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