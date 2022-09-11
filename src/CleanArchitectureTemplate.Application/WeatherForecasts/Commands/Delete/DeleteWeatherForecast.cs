﻿using CleanArchitectureTemplate.Domain.Common.Database.Repositories;
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
        private readonly IWeatherForecastRepository _context;

        public DeleteWeatherForecastHandler(IWeatherForecastRepository context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteWeatherForecast request, CancellationToken cancellationToken)
        {
            var entity = await _context.Get(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(WeatherForecast), request.Id.ToString());
            }

            await _context.Remove(entity);
            await _context.SaveChanges();

            return Unit.Value;
        }
    }
}