using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.Queries;

public class ViewInvitations
{
    public record Query(string UserId, int PageNumber, int DisplayedRows, int RowSize) : IQuery<Answer>;
    
    public record Answer(List<ViaInvitation> Invitations, int InvitationsCount);
    
    public record ViaInvitation(
        string Id,
        string EventId,
        string EventTitle,
        string EventFrom,
        string EventTo,
        string EventVisibility,
        string InvitationStatus
    );
}