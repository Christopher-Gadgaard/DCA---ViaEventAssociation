using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class UnpublishedEvents
{
    public record Query() : IQuery<Answer>;

    public record Answer(
        List<Event> DraftEvents,
        List<Event> ReadyEvents,
        List<Event> CancelledEvents
    );
        
    public record Event(
        string Id,
        string Title,
        string Status
    );
}