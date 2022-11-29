namespace CleanArchitectureTemplate.Domain.Common
{
    public abstract class AggregateRoot : Entity { }
    public abstract class AggregateRoot<Tid> : Entity<Tid> where Tid : notnull
    {
        protected AggregateRoot(Tid id) : base(id)
        { }
    }
}
