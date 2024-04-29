using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public abstract class ProfilePage
{
    public record Query(string GuestId) : IQuery<Answer>;

    public record Answer(Guest Guest, IEnumerable<Event> Events, int GuestPendingInvitationsCount);
    
    public record Event(string Id, string Name, int GuestCount, DateTime Date);
    
    public record Guest(string Id, string FirstName, string LastName, string Email, string PictureUrl);
    
}