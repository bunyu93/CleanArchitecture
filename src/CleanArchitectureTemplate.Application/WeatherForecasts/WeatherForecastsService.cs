using CleanArchitectureTemplate.Application.WeatherForecasts.Mappings;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Results;
using CleanArchitectureTemplate.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts;

public interface IWeatherForecastsService
{
    Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAll(CancellationToken cancellationToken = default);

    Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAllEf(CancellationToken cancellationToken = default);

    Task<Result<WeatherForecastQueryModel>> GetById(int id, CancellationToken cancellationToken = default);

    Task<Result> Create(WeatherForecastCreateModel payload, CancellationToken cancellationToken = default);

    Task<Result> Update(WeatherForecastUpdateModel payload, CancellationToken cancellationToken = default);

    Task<Result> Delete(int id, CancellationToken cancellationToken = default);
}

public class WeatherForecastsService(IUnitOfWork unitOfWork) : IWeatherForecastsService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAll(CancellationToken cancellationToken = default)
    {
        IQueryable<WeatherForecastQueryModel>? result =
            await _unitOfWork.SqlQuery<WeatherForecastQueryModel>($"SELECT * FROM weather.forecast");

        if (result is null)
        {
            return Result.Failure<IEnumerable<WeatherForecastQueryModel>>(ResultError.NotFound("404",
                "Cannot get the entities"));
        }

        return Result.Success<IEnumerable<WeatherForecastQueryModel>>(result);
    }

    public async Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAllEf(CancellationToken cancellationToken = default)
    {
        Result<IEnumerable<WeatherForecast>> WeatherForecasts = await _unitOfWork.WeatherForecastRepository.GetAll(cancellationToken);
        IEnumerable<WeatherForecastQueryModel> result = WeatherForecasts.Value.Select(x => x.MapToQueryModel());

        return Result.Success<IEnumerable<WeatherForecastQueryModel>>(result);
    }

    public async Task<Result<WeatherForecastQueryModel>> GetById(int id, CancellationToken cancellationToken = default)
    {
        WeatherForecastQueryModel? result =
            (await _unitOfWork.SqlQuery<WeatherForecastQueryModel>(
                $"SELECT * FROM weather.forecast WHERE id = {id}")).FirstOrDefault();

        if (result is null)
        {
            return Result.Failure<WeatherForecastQueryModel>(ResultError.NotFound("404", "Cannot get the entities"));
        }

        return Result.Success<WeatherForecastQueryModel>(result);
    }

    public async Task<Result> Create(WeatherForecastCreateModel payload, CancellationToken cancellationToken = default)
    {
        WeatherForecast entity = new()
        {
            Date = payload.Date,
            Temperature = new Temperature
            {
                Celsius = payload.Temperature.Celsius, Fahrenheit = payload.Temperature.Fahrenheit
            },
            Summary = payload.Summary
        };

        await _unitOfWork.WeatherForecastRepository.Add(entity, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> Update(WeatherForecastUpdateModel payload, CancellationToken cancellationToken = default)
    {
        int id = payload.Id;
        Result<WeatherForecast> entityCurrent = await _unitOfWork.WeatherForecastRepository.GetById(id, cancellationToken);

        if (!entityCurrent.IsSuccess)
        {
            return Result.Failure(
                ResultError.NotFound("404", $"{nameof(WeatherForecast)} id with {id} cannot be found"));
        }

        WeatherForecast entityUpdated = entityCurrent.Value;
        entityUpdated.Id = id;
        entityUpdated.Date = DateTime.UtcNow;
        entityUpdated.Temperature = new Temperature
        {
            Celsius = payload.Temperature.Celsius, Fahrenheit = payload.Temperature.Fahrenheit
        };
        entityUpdated.Summary = payload.Summary;

        await _unitOfWork.WeatherForecastRepository.Update(entityCurrent.Value, entityUpdated);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> Delete(int id, CancellationToken cancellationToken = default)
    {
        Result<WeatherForecast> entity = await _unitOfWork.WeatherForecastRepository.GetById(id, cancellationToken);

        if (!entity.IsSuccess)
        {
            return Result.Failure(
                ResultError.NotFound("404", $"{nameof(WeatherForecast)} id with {id} cannot be found"));
        }

        await _unitOfWork.WeatherForecastRepository.Remove(entity.Value);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
