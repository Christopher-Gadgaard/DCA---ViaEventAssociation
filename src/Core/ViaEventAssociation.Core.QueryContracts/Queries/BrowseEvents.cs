using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class BrowseEvents
{
    public record Query(int PageNumber, int PageSize,string SearchedText) : IQuery<Answer>;

    public record Answer(List<Event> Events, int MaxPageNum);

    public record Event(
        string Id,
        string Title,
        string Description,
        int MaxGuests,
        int GuestCount,
        string From);
}