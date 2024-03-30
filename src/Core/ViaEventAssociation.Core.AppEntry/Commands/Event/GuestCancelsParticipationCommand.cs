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
        OperationResult<ViaEventId> eventResult = ViaEventId.Create();
        OperationResult<ViaGuestId> guestResult = ViaGuestId.Create();
        OperationResult<GuestCancelsParticipationCommand> combinedResult = OperationResult<GuestCancelsParticipationCommand>.Combine(eventResult.OperationErrors, guestResult.OperationErrors);

        return combinedResult;
    }
}