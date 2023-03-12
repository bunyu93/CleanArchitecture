using System;

namespace CleanArchitectureTemplate.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message = "validations failed") : base(message)
    {
    }
}