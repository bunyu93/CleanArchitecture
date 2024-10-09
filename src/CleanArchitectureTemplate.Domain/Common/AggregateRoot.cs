namespace CleanArchitectureTemplate.Domain.Common;

public abstract class AggregateRoot : Entity
{ }

public abstract class AggregateRoot<Tid>(Tid id) : Entity<Tid>(id) where Tid : notnull
{
}