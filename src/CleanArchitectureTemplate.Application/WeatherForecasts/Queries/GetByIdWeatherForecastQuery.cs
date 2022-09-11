using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Exceptions;
using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Queries
{
    public record GetByIdWeatherForecastQuery(int Id) : IRequest<QueryModelWeatherForecast>;

    public class GetByIdWeatherForecastQueryHandler : IRequestHandler<GetByIdWeatherForecastQuery, QueryModelWeatherForecast>
    {
        private readonly IDapperContext _context;

        public GetByIdWeatherForecastQueryHandler(IDapperContext context)
        {
            _context = context;
        }

        public async Task<QueryModelWeatherForecast> Handle(GetByIdWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            var parameters = new { Id = request.Id };

            using var db = _context.CreateConnection();
            db.Open();
            var data = await db.QueryFirstOrDefaultAsync<QueryModelWeatherForecast>("SELECT * FROM WeatherForecast WHERE id = @Id", parameters) ?? throw new NotFoundException();
            db.Close();

            return data;
        }
    }
}