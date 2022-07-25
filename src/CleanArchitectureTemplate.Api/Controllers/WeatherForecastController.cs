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
            return await Mediator.Send(new GetWeatherForecastsQuery());
        }

        [HttpGet("{id}")]
        public async Task<QueryModelWeatherForecast> Get([FromRoute] int id)
        {
            return await Mediator.Send(new GetByIdWeatherForecastsQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateModelWeatherForecasts request)
        {
            await Mediator.Send(new CreateWeatherForecasts(request));

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateModelWeatherForecasts request)
        {
            await Mediator.Send(new UpdateWeatherForecasts(id, request));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await Mediator.Send(new DeleteWeatherForecasts(id));

            return NoContent();
        }
    }
}