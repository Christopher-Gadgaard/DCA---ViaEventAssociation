using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventSetMaxGuestsCommand
{
    public ViaEventId Id { get; }
    public ViaMaxGuests MaxGuests { get; }

    public static OperationResult<ViaEventSetMaxGuestsCommand> Create(string id, int maxGuest)
    {
        var eventId = ViaEventId.CreateFromString(id);
        var mGuests = ViaMaxGuests.Create(maxGuest);
        var combinedResult =
            OperationResult<ViaEventSetMaxGuestsCommand>.Combine(eventId.OperationErrors, mGuests.OperationErrors);
        return combinedResult.OperationErrors.Count > 0
            ? combinedResult
            : new ViaEventSetMaxGuestsCommand(eventId.Payload, mGuests.Payload);
    }

    private ViaEventSetMaxGuestsCommand(ViaEventId id, ViaMaxGuests maxGuest) => (Id, MaxGuests) = (id, maxGuest);
}