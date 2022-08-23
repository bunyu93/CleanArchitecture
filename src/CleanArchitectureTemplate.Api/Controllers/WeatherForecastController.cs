using CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Create;
using CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Delete;
using CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Update;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using CleanArchitectureTemplate.Application.WeatherForecasts.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Api.Controllers
{
    public class WeatherForecastController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IEnumerable<QueryModelWeatherForecast>> Get()
        {
            return await Mediator.Send(new GetWeatherForecastQuery());
        }

        [HttpGet("{id}")]
        public async Task<QueryModelWeatherForecast> Get([FromRoute] int id)
        {
            return await Mediator.Send(new GetByIdWeatherForecastQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateModelWeatherForecast request)
        {
            await Mediator.Send(new CreateWeatherForecast(request));

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateModelWeatherForecast request)
        {
            await Mediator.Send(new UpdateWeatherForecast(id, request));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteWeatherForecast(id));

            return NoContent();
        }
    }
}