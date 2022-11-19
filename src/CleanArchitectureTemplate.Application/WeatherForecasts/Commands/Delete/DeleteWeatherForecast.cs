using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Exceptions;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Delete
{
    public record DeleteWeatherForecast(int Id) : IRequest;

    public class DeleteWeatherForecastHandler : IRequestHandler<DeleteWeatherForecast>
    {
        private readonly IUnitOfWork _context;

        public DeleteWeatherForecastHandler(IUnitOfWork context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteWeatherForecast request, CancellationToken cancellationToken)
        {
            var entity = await _context.WeatherForecastRepository.Get(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(WeatherForecast), request.Id.ToString());
            }

            await _context.WeatherForecastRepository.Remove(entity);
            await _context.SaveAsync();

            return Unit.Value;
        }
    }
}