using System.Collections.Generic;

namespace CleanArchitectureTemplate.Domain.ValueObjects;

public class Temperature : ValueObject
{
    public int Fahrenheit { get; init; }
    public int Celsius { get; init; }

    public Temperature()
    { }

    public Temperature(int temperature)
    {
        Fahrenheit = 32 + (int)(temperature / 0.5556);
        Celsius = temperature;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Fahrenheit;
        yield return Celsius;
    }
}