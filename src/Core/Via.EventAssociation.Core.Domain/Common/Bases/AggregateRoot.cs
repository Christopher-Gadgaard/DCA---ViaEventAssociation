namespace Via.EventAssociation.Core.Domain.Common.Bases;

public abstract class AggregateRoot<T> : Entity<T>
{
    protected AggregateRoot(T id) : base(id)
    {
    }

    protected AggregateRoot()
    {
    }
}