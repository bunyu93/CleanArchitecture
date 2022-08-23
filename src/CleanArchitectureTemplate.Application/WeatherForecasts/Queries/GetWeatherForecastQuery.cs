using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Queries
{
    public record GetWeatherForecastQuery : IRequest<IEnumerable<QueryModelWeatherForecast>>;

    public class GetWeatherForecastQueryHandler : IRequestHandler<GetWeatherForecastQuery, IEnumerable<QueryModelWeatherForecast>>
    {
        private readonly IDapperContext _context;

        public GetWeatherForecastQueryHandler(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QueryModelWeatherForecast>> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            using var db = _context.CreateConnection();
            db.Open();
            var data = await db.QueryAsync<QueryModelWeatherForecast>("SELECT * FROM WeatherForecast");
            db.Close();

            return data.AsList();
        }
    }
}