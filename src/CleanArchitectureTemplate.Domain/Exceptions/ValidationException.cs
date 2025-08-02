using System;

namespace CleanArchitectureTemplate.Domain.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
    {
    }

    public ValidationException(string message, Exception innerException) : base(message, innerException)
    {
    }
    public ValidationException(string message = "validations failed") : base(message)
    {
    }
}