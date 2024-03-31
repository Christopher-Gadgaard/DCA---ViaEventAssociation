namespace Via.EventAssociation.Core.Domain.Contracts;

public interface ITimeProvider
{
    DateTime Now { get; }
}