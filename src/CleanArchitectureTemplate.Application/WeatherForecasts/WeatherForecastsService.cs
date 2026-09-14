using CleanArchitectureTemplate.Application.WeatherForecasts.Mappings;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Results;
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
        IQueryable<WeatherForecastQueryModel> result =
            await _unitOfWork.SqlQuery<WeatherForecastQueryModel>($"SELECT * FROM weather.forecast");

        return Result.Success<IEnumerable<WeatherForecastQueryModel>>(result);
    }

    public async Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAllEf(CancellationToken cancellationToken = default)
    {
        Result<IEnumerable<WeatherForecast>> weatherForecasts = await _unitOfWork.WeatherForecastRepository.GetAll(cancellationToken);

        if (!weatherForecasts.IsSuccess)
        {
            return Result.Failure<IEnumerable<WeatherForecastQueryModel>>(weatherForecasts.Error!);
        }

        IEnumerable<WeatherForecastQueryModel> result = weatherForecasts.Value.Select(x => x.MapToQueryModel());
        return Result.Success<IEnumerable<WeatherForecastQueryModel>>(result);
    }

    public async Task<Result<WeatherForecastQueryModel>> GetById(int id, CancellationToken cancellationToken = default)
    {
        WeatherForecastQueryModel? result =
            (await _unitOfWork.SqlQuery<WeatherForecastQueryModel>(
                $"SELECT * FROM weather.forecast WHERE id = {id}")).FirstOrDefault();

        if (result is null)
        {
            return Result.Failure<WeatherForecastQueryModel>(
                ResultError.NotFound("WeatherForecast.NotFound", $"{nameof(WeatherForecast)} with id {id} was not found"));
        }

        return Result.Success<WeatherForecastQueryModel>(result);
    }

    public async Task<Result> Create(WeatherForecastCreateModel payload, CancellationToken cancellationToken = default)
    {
        WeatherForecast entity = payload.MapToWeatherForecast();

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
                ResultError.NotFound("WeatherForecast.NotFound", $"{nameof(WeatherForecast)} with id {id} was not found"));
        }

        WeatherForecast entityUpdated = payload.MapToWeatherForecast();

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
                ResultError.NotFound("WeatherForecast.NotFound", $"{nameof(WeatherForecast)} with id {id} was not found"));
        }

        await _unitOfWork.WeatherForecastRepository.Remove(entity.Value);
        await _unitOfWork.SaveAsync(cancellationToken);

        return Result.Success();
    }
}
