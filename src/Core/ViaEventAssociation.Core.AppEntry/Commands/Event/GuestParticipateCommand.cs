using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class GuestParticipateCommand
{
    public ViaEventId EventId { get; }
    public ViaGuestId GuestId { get; }

    public GuestParticipateCommand(ViaEventId eventId, ViaGuestId guestId)
    {
        EventId = eventId;
        GuestId = guestId;
    }

    public static OperationResult<GuestParticipateCommand> Create(Guid eventId, Guid guestId)
    {
        OperationResult<ViaEventId> eventResult = ViaEventId.CreateFromString(eventId.ToString());
        OperationResult<ViaGuestId> guestResult = ViaGuestId.Create(guestId.ToString());
        if (eventResult.IsSuccess && guestResult.IsSuccess)
        {
            return OperationResult<GuestParticipateCommand>.Success(
                new GuestParticipateCommand(eventResult.Payload, guestResult.Payload));
        }

        return OperationResult<GuestParticipateCommand>.Combine(eventResult.OperationErrors,
            guestResult.OperationErrors);
    }
}