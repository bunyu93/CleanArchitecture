using CleanArchitectureTemplate.Application.WeatherForecasts.Mappings;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Results;
using CleanArchitectureTemplate.Domain.ValueObjects;
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
        var result = await _unitOfWork.SqlQuery<WeatherForecastQueryModel>($"SELECT * FROM WeatherForecast");

        if (result is null)
            return Result<IEnumerable<WeatherForecastQueryModel>>.Failure(ResultError.NotFound("404", "Cannot get the entities"));
        else
            return Result<IEnumerable<WeatherForecastQueryModel>>.Success(result);
    }

    public async Task<Result<IEnumerable<WeatherForecastQueryModel>>> GetAllEf()
    {
        var WeatherForecasts = await _unitOfWork.WeatherForecastRepository.GetAll();
        var result = WeatherForecasts.Value.Select(x => x.MapToQueryModel());

        return Result<IEnumerable<WeatherForecastQueryModel>>.Success(result);
    }

    public async Task<Result<WeatherForecastQueryModel>> GetById(int id)
    {
        var result = (await _unitOfWork.SqlQuery<WeatherForecastQueryModel>($"SELECT * FROM WeatherForecast WHERE id = {id}")).FirstOrDefault();

        if (result is null)
            return Result<WeatherForecastQueryModel>.Failure(ResultError.NotFound("404", "Cannot get the entities"));
        else
            return Result<WeatherForecastQueryModel>.Success(result);
    }

    public async Task<Result> Create(WeatherForecastCreateModel payload)
    {
        var entity = new WeatherForecast()
        {
            Date = payload.Date,
            Temperature = new Temperature()
            {
                Celsius = payload.Temperature.Celsius,
                Fahrenheit = payload.Temperature.Fahrenheit
            },
            Summary = payload.Summary
        };

        await _unitOfWork.WeatherForecastRepository.Add(entity);
        await _unitOfWork.SaveAsync();

        return Result.Success();
    }

    public async Task<Result> Update(WeatherForecastUpdateModel payload)
    {
        var id = payload.Id;
        var entityCurrent = await _unitOfWork.WeatherForecastRepository.GetById(id);

        if (!entityCurrent.IsSuccess)
        {
            return Result.Failure(ResultError.NotFound("404", $"{nameof(WeatherForecast)} id with {id} cannot be found"));
        }

        var entityUpdated = entityCurrent.Value;
        entityUpdated.Id = id;
        entityUpdated.Date = System.DateTime.Now;
        entityUpdated.Temperature = new Temperature()
        {
            Celsius = payload.Temperature.Celsius,
            Fahrenheit = payload.Temperature.Fahrenheit
        };
        entityUpdated.Summary = payload.Summary;

        await _unitOfWork.WeatherForecastRepository.Update(entityCurrent.Value, entityUpdated);
        await _unitOfWork.SaveAsync();

        return Result.Success();
    }

    public async Task<Result> Delete(int id)
    {
        var entity = (await _unitOfWork.WeatherForecastRepository.GetById(id));

        if (!entity.IsSuccess)
        {
            return Result.Failure(ResultError.NotFound("404", $"{nameof(WeatherForecast)} id with {id} cannot be found"));
        }

        await _unitOfWork.WeatherForecastRepository.Remove(entity.Value);
        await _unitOfWork.SaveAsync();

        return Result.Success();
    }
}