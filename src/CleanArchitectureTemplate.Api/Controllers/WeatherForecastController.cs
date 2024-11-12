using CleanArchitectureTemplate.Application.WeatherForecasts;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Domain.Result;
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
    public async Task<IActionResult> Get()
    {
        var result = await _weatherForecastsService.GetAll();

        return result.Match(
            onSuccess: Ok,
            onFailure: Problem
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WeatherForecastQueryModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await _weatherForecastsService.GetById(id);

        return result.Match(
            onSuccess: Ok,
            onFailure: Problem
        );
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Post([FromBody] WeatherForecastCreateModel request)
    {
        var result = await _weatherForecastsService.Create(request);

        return result.Match(
           onSuccess: NoContent,
           onFailure: Problem
       );
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Put([FromRoute] int id, [FromBody] WeatherForecastUpdateModel request)
    {
        var result = await _weatherForecastsService.Update(id, request);

        return result.Match(
           onSuccess: NoContent,
           onFailure: Problem
       );
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var result = await _weatherForecastsService.Delete(id);

        return result.Match(
           onSuccess: NoContent,
           onFailure: Problem
       );
    }
}