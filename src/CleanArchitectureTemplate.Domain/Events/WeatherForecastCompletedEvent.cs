namespace CleanArchitectureTemplate.Domain.Events
{
    public class WeatherForecastCompletedEvent : BaseEvent
    {
        public WeatherForecastCompletedEvent(WeatherForecast weather)
        {
            Weather = weather;
        }

        public WeatherForecast Weather { get; }
    }
}