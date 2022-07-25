using FluentValidation;

namespace CleanArchitectureTemplate.Application.WeatherForecasts.Commands.Update
{
    public class UpdateWeatherForecastsValidator : AbstractValidator<UpdateWeatherForecasts>
    {
        public UpdateWeatherForecastsValidator()
        {
            RuleFor(v => v.WeatherForecast.Date)
               .NotEmpty()
               .NotNull();

            RuleFor(v => v.WeatherForecast.Temperature.Fahrenheit)
               .NotEmpty()
               .NotNull();

            RuleFor(v => v.WeatherForecast.Temperature.Celsius)
               .NotEmpty()
               .NotNull();

            RuleFor(v => v.WeatherForecast.Summary)
                .MaximumLength(255)
                .NotEmpty()
                .NotNull();
        }
    }
}