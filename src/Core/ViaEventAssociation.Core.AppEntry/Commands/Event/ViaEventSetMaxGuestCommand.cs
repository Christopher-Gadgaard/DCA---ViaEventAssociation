using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventSetMaxGuestCommand
{
    public ViaEventId Id { get; }
    public ViaMaxGuests MaxGuest { get; }
    
    public static OperationResult<ViaEventSetMaxGuestCommand> Create(string id, int maxGuest)
    {
        var eventId = ViaEventId.CreateFromString(id);
        var mGuests = ViaMaxGuests.Create(maxGuest);
        var combinedResult = OperationResult<ViaEventSetMaxGuestCommand>.Combine(eventId.OperationErrors, mGuests.OperationErrors);
        return combinedResult.OperationErrors.Count>0 ? combinedResult : new ViaEventSetMaxGuestCommand(eventId.Payload, mGuests.Payload);
    }
    
    private ViaEventSetMaxGuestCommand(ViaEventId id, ViaMaxGuests maxGuest) => (Id, MaxGuest) = (id, maxGuest);
}