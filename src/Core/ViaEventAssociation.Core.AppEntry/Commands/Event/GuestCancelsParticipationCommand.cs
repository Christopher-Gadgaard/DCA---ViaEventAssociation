using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class GuestCancelsParticipationCommand
{
    public ViaEventId EventId { get;}
    public ViaGuestId GuestId { get;}
    
    public GuestCancelsParticipationCommand(ViaEventId eventId, ViaGuestId guestId)
    {
        EventId = eventId;
        GuestId = guestId;
    }
    
    public static OperationResult<GuestCancelsParticipationCommand> Create(string eventId, string guestId)
    {
        OperationResult<ViaEventId> eventResult = ViaEventId.CreateFromString(eventId);
        OperationResult<ViaGuestId> guestResult = ViaGuestId.Create(guestId);
        
        if( eventResult.IsSuccess && guestResult.IsSuccess)
        {
            return OperationResult<GuestCancelsParticipationCommand>.Success(new GuestCancelsParticipationCommand(eventResult.Payload, guestResult.Payload));
        }

        return OperationResult<GuestCancelsParticipationCommand>.Combine(eventResult.OperationErrors, guestResult.OperationErrors);
    }
}