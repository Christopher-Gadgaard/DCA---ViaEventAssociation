namespace Via.EventAssociation.Core.Domain.Common.Bases;

public abstract class Entity<TViaId> where TViaId : ValueObject
{
    public TViaId Id { get; protected set; }

    protected Entity(TViaId id)
    {
        Id = id;
    }

    protected Entity()
    {
    }
}