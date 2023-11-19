using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Domain.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitectureTemplate.Application.WeatherForecasts;

public interface IWeatherForecastsService
{
    Task<IEnumerable<WeatherForecastQueryModel>> GetAll();

    Task<WeatherForecastQueryModel> GetById(int id);

    Task Create(WeatherForecastCreateModel payload);

    Task Update(int id, WeatherForecastUpdateModel payload);

    Task Delete(int id);
}

public class WeatherForecastsService : IWeatherForecastsService
{
    private readonly IUnitOfWork _unitOfWork;

    public WeatherForecastsService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<WeatherForecastQueryModel>> GetAll()
        => await _unitOfWork.SqlQuery<WeatherForecastQueryModel>($"SELECT * FROM WeatherForecast");

    public async Task<WeatherForecastQueryModel> GetById(int id)
        => (await _unitOfWork.SqlQuery<WeatherForecastQueryModel>($"SELECT * FROM WeatherForecast WHERE id = {id}")).FirstOrDefault() ??
            throw new NotFoundException("WeatherForecast", id.ToString());

    public async Task Create(WeatherForecastCreateModel payload)
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
    }

    public async Task Update(int id, WeatherForecastUpdateModel payload)
    {
        var entityCurrent = await _unitOfWork.WeatherForecastRepository.Get(id) ?? throw new NotFoundException(nameof(WeatherForecast), id.ToString());

        var entityUpdated = entityCurrent;
        entityUpdated.Id = id;
        entityUpdated.Date = System.DateTime.Now;
        entityUpdated.Temperature = new Temperature()
        {
            Celsius = payload.Temperature.Celsius,
            Fahrenheit = payload.Temperature.Fahrenheit
        };
        entityUpdated.Summary = payload.Summary;

        await _unitOfWork.WeatherForecastRepository.Update(entityCurrent, entityUpdated);
        await _unitOfWork.SaveAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.WeatherForecastRepository.Get(id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(WeatherForecast), id.ToString());
        }

        await _unitOfWork.WeatherForecastRepository.Remove(entity);
        await _unitOfWork.SaveAsync();
    }
}