using CleanArchitectureTemplate.Application.WeatherForecasts.Mappings;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Results;
using CleanArchitectureTemplate.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts;

public interface IWeatherForecastsService
{
    Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAll();

    Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAllEf();

    Task<Result<WeatherForecastQueryModel>> GetById(int id);

    Task<Result> Create(WeatherForecastCreateModel payload);

    Task<Result> Update(WeatherForecastUpdateModel payload);

    Task<Result> Delete(int id);
}

public class WeatherForecastsService(IUnitOfWork unitOfWork) : IWeatherForecastsService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAll()
    {
        IQueryable<WeatherForecastQueryModel>? result =
            await _unitOfWork.SqlQuery<WeatherForecastQueryModel>($"SELECT * FROM \"public.weather_forecast\"");

        if (result is null)
        {
            return Result<IEnumerable<WeatherForecastQueryModel>>.Failure(ResultError.NotFound("404",
                "Cannot get the entities"));
        }

        return Result<IEnumerable<WeatherForecastQueryModel>>.Success(result);
    }

    public async Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAllEf()
    {
        Result<IEnumerable<WeatherForecast>> WeatherForecasts = await _unitOfWork.WeatherForecastRepository.GetAll();
        IEnumerable<WeatherForecastQueryModel> result = WeatherForecasts.Value.Select(x => x.MapToQueryModel());

        return Result<IEnumerable<WeatherForecastQueryModel>>.Success(result);
    }

    public async Task<Result<WeatherForecastQueryModel>> GetById(int id)
    {
        WeatherForecastQueryModel? result =
            (await _unitOfWork.SqlQuery<WeatherForecastQueryModel>(
                $"SELECT * FROM \"public.weather_forecast\" WHERE \"Id\" = {id}")).FirstOrDefault();

        if (result is null)
        {
            return Result<WeatherForecastQueryModel>.Failure(ResultError.NotFound("404", "Cannot get the entities"));
        }

        return Result<WeatherForecastQueryModel>.Success(result);
    }

    public async Task<Result> Create(WeatherForecastCreateModel payload)
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

        await _unitOfWork.WeatherForecastRepository.Add(entity);
        await _unitOfWork.SaveAsync();

        return Result.Success();
    }

    public async Task<Result> Update(WeatherForecastUpdateModel payload)
    {
        int id = payload.Id;
        Result<WeatherForecast> entityCurrent = await _unitOfWork.WeatherForecastRepository.GetById(id);

        if (!entityCurrent.IsSuccess)
        {
            return Result.Failure(
                ResultError.NotFound("404", $"{nameof(WeatherForecast)} id with {id} cannot be found"));
        }

        WeatherForecast entityUpdated = entityCurrent.Value;
        entityUpdated.Id = id;
        entityUpdated.Date = DateTime.Now;
        entityUpdated.Temperature = new Temperature
        {
            Celsius = payload.Temperature.Celsius, Fahrenheit = payload.Temperature.Fahrenheit
        };
        entityUpdated.Summary = payload.Summary;

        await _unitOfWork.WeatherForecastRepository.Update(entityCurrent.Value, entityUpdated);
        await _unitOfWork.SaveAsync();

        return Result.Success();
    }

    public async Task<Result> Delete(int id)
    {
        Result<WeatherForecast> entity = await _unitOfWork.WeatherForecastRepository.GetById(id);

        if (!entity.IsSuccess)
        {
            return Result.Failure(
                ResultError.NotFound("404", $"{nameof(WeatherForecast)} id with {id} cannot be found"));
        }

        await _unitOfWork.WeatherForecastRepository.Remove(entity.Value);
        await _unitOfWork.SaveAsync();

        return Result.Success();
    }
}