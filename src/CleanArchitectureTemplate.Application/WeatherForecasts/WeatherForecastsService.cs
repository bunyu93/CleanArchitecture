using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Common.Database;
using CleanArchitectureTemplate.Domain.Entities;
using CleanArchitectureTemplate.Domain.Exceptions;
using CleanArchitectureTemplate.Domain.ValueObjects;
using Dapper;
using System.Collections.Generic;
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
    private readonly IDapperContext _dapper;

    public WeatherForecastsService(IUnitOfWork unitOfWork, IDapperContext dapper)
    {
        _unitOfWork = unitOfWork;
        _dapper = dapper;
    }

    public async Task<IEnumerable<WeatherForecastQueryModel>> GetAll()
    {
        using var db = _dapper.CreateConnection();
        db.Open();
        var data = await db.QueryAsync<WeatherForecastQueryModel>("SELECT * FROM WeatherForecast") ?? throw new NotFoundException();
        db.Close();

        return data.AsList();
    }

    public async Task<WeatherForecastQueryModel> GetById(int id)
    {
        var parameters = new { Id = id };

        using var db = _dapper.CreateConnection();
        db.Open();
        var data = await db.QueryFirstOrDefaultAsync<WeatherForecastQueryModel>("SELECT * FROM WeatherForecast WHERE id = @Id", parameters) ?? throw new NotFoundException(id.ToString());
        db.Close();

        return data;
    }

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