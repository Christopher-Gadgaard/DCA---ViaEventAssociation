using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class GuestParticipateCommand
{
    public ViaEventId EventId { get;}
    public ViaGuestId GuestId { get;}
    
    public GuestParticipateCommand(ViaEventId eventId, ViaGuestId guestId)
    {
        EventId = eventId;
        GuestId = guestId;
    }
    
    public static OperationResult<GuestParticipateCommand> Create(string eventId, string guestId)
    {
        OperationResult<ViaEventId> eventResult = ViaEventId.Create();
        OperationResult<ViaGuestId> guestResult = ViaGuestId.Create();
        OperationResult<GuestParticipateCommand> combinedResult = OperationResult<GuestParticipateCommand>.Combine(eventResult.OperationErrors, guestResult.OperationErrors);

        return combinedResult;
    }
}