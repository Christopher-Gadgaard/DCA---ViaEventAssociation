using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class ViewEvent
{
    public record Query(string EventId, int PageNumber, int DisplayedRows, int RowSize) : IQuery<Answer>;
    public record Answer(Event Event, List<Guest> Guest, int GuestsCount);

    public record Event(
        string Title,
        string Description,
        string From,
        string To,
        string Visibility,
        int MaxGuests
    );
    
    public record Guest(
        string Id,
        string FirstName,
        string LastName,
        string PictureUrl
    );
}