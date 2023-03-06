using CleanArchitectureTemplate.Application.WeatherForecasts;
using CleanArchitectureTemplate.Application.WeatherForecasts.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitectureTemplate.Api.Controllers
{
    [Route("weather-forecast")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IWeatherForecastsService _weatherForecastsService;

        public WeatherForecastController(IWeatherForecastsService weatherForecastsService)
        {
            _weatherForecastsService = weatherForecastsService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecastQueryModel>> Get()
        {
            return await _weatherForecastsService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<WeatherForecastQueryModel> Get([FromRoute] int id)
        {
            return await _weatherForecastsService.GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] WeatherForecastCreateModel request)
        {
            await _weatherForecastsService.Create(request);

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] WeatherForecastUpdateModel request)
        {
            await _weatherForecastsService.Update(id, request);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await _weatherForecastsService.Delete(id);

            return NoContent();
        }
    }
}