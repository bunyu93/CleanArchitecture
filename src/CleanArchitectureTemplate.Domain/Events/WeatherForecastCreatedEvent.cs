namespace CleanArchitectureTemplate.Domain.Events
{
    public class WeatherForecastCreatedEvent : BaseEvent
    {
        public WeatherForecastCreatedEvent(WeatherForecast weather)
        {
            Weather = weather;
        }

        public WeatherForecast Weather { get; }
    }
}