using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Queries
{
    public record GetWeatherForecastsQuery : IRequest<IEnumerable<QueryModelWeatherForecast>>;

    public class GetWeatherForecastsQueryHandler : IRequestHandler<GetWeatherForecastsQuery, IEnumerable<QueryModelWeatherForecast>>
    {
        private readonly IDapperContext _context;

        public GetWeatherForecastsQueryHandler(IDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QueryModelWeatherForecast>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
        {
            using var db = _context.CreateConnection();
            db.Open();
            var data = await db.QueryAsync<QueryModelWeatherForecast>("SELECT * FROM WeatherForecast");
            db.Close();

            return data.AsList();
        }
    }
}