using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventUpdateDescriptionCommand
{
    public ViaEventId Id { get; }
    public ViaEventDescription Description { get; }

    public static OperationResult<ViaEventUpdateDescriptionCommand> Create(string id, string description)
    {
        var eventId = ViaEventId.CreateFromString(id);
        var eventDescription = ViaEventDescription.Create(description);
        var combinedResult = OperationResult<ViaEventUpdateDescriptionCommand>.Combine(eventId.OperationErrors, eventDescription.OperationErrors);
        return combinedResult.OperationErrors.Count>0 ? combinedResult : new ViaEventUpdateDescriptionCommand(eventId.Payload, eventDescription.Payload);
    }
    
    private ViaEventUpdateDescriptionCommand(ViaEventId id, ViaEventDescription description) => (Id, Description) = (id, description);
}