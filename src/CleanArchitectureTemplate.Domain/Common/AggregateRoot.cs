namespace CleanArchitectureTemplate.Domain.Common;

public abstract class AggregateRoot : Entity
{ }

public abstract class AggregateRoot<TId>(TId id) : Entity<TId>(id) where TId : notnull
{
}