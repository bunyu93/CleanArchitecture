using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Persistence.Repo;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Delete
{
    public record DeleteWeatherForecasts(int Id) : IRequest;

    public class DeleteWeatherForecastsHandler : IRequestHandler<DeleteWeatherForecasts>
    {
        private readonly IRepository<WeatherForecast> _context;

        public DeleteWeatherForecastsHandler(IRepository<WeatherForecast> context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteWeatherForecasts request, CancellationToken cancellationToken)
        {
            var entity = await _context.Get(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(WeatherForecast), request.Id);
            }

            await _context.Remove(entity);
            await _context.SaveChanges();

            return Unit.Value;
        }
    }
}