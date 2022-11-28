namespace CleanArchitectureTemplate.Domain.Common
{
    public abstract class AggregateRoot<T> : Entity<T> where T : notnull
    {
        protected AggregateRoot(T id) : base(id)
        { }
    }
}
