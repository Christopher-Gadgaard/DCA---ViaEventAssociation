using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class GuestAcceptsInvitationCommand
{
    public ViaEventId EventId { get;}
    public ViaGuestId GuestId { get;}
    public ViaInvitationId InvitationId { get;}
    
    public GuestAcceptsInvitationCommand(ViaEventId eventId, ViaGuestId guestId, ViaInvitationId invitationId)
    {
        EventId = eventId;
        GuestId = guestId;
        InvitationId = invitationId;
    }
    
    public static OperationResult<GuestAcceptsInvitationCommand> Create(string eventId, string guestId, string invitationId)
    {
        OperationResult<ViaEventId> eventResult = ViaEventId.CreateFromString(eventId);
        OperationResult<ViaGuestId> guestResult = ViaGuestId.Create(guestId);
        OperationResult<ViaInvitationId> invitationResult = ViaInvitationId.Create(invitationId);
        OperationResult<GuestAcceptsInvitationCommand> combinedResult = OperationResult<GuestAcceptsInvitationCommand>.Combine(eventResult.OperationErrors, guestResult.OperationErrors, invitationResult.OperationErrors);

        if(combinedResult.IsSuccess)
        {
            return OperationResult<GuestAcceptsInvitationCommand>.Success(new GuestAcceptsInvitationCommand(eventResult.Payload, guestResult.Payload, invitationResult.Payload));
        }
        return combinedResult;
    }
}