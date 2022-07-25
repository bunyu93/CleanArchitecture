using System;

namespace CleanArchitectureTemplate.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base($"Entity was not found.")
        {
        }

        public NotFoundException(string name, object key) : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}