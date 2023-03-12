using System;

namespace CleanArchitectureTemplate.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException() : base($"Entity was not found.")
    {
    }

    public NotFoundException(string param) : base($"Entity with params: '{param}' was not found.")
    {
    }

    public NotFoundException(string name, string id) : base($"Entity '{name}' with id: '{id}' was not found.")
    {
    }
}