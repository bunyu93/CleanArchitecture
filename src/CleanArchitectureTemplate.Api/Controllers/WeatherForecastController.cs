using CleanArchitectureTemplate.Application.WeatherForecasts;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Results;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Api.Controllers;

[ApiController]
[Route("weather-forecast")]
public class WeatherForecastController(IWeatherForecastsService weatherForecastsService) : Controller
{
    private readonly IWeatherForecastsService _weatherForecastsService = weatherForecastsService;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecastQueryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _weatherForecastsService.GetAll(cancellationToken);

        return result.Match(
            onSuccess: Ok,
            onFailure: Problem
        );
    }

    [HttpGet("ef")]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecastQueryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEf(CancellationToken cancellationToken)
    {
        var result = await _weatherForecastsService.GetAllEf(cancellationToken);

        return result.Match(
            onSuccess: Ok,
            onFailure: Problem
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WeatherForecastQueryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _weatherForecastsService.GetById(id, cancellationToken);

        return result.Match(
            onSuccess: Ok,
            onFailure: Problem
        );
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post([FromBody] WeatherForecastCreateModel request, CancellationToken cancellationToken)
    {
        var result = await _weatherForecastsService.Create(request, cancellationToken);

        return result.Match(
           onSuccess: NoContent,
           onFailure: Problem
       );
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromBody] WeatherForecastUpdateModel request, CancellationToken cancellationToken)
    {
        var result = await _weatherForecastsService.Update(request, cancellationToken);

        return result.Match(
           onSuccess: NoContent,
           onFailure: Problem
       );
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _weatherForecastsService.Delete(id, cancellationToken);

        return result.Match(
           onSuccess: NoContent,
           onFailure: Problem
       );
    }
}