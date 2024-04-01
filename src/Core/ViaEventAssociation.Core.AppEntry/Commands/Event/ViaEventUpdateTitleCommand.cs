using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventUpdateTitleCommand
{
    public ViaEventId Id { get; }
    public ViaEventTitle Title { get; }

    public static OperationResult<ViaEventUpdateTitleCommand> Create(string id, string title)
    {
        var eventId = ViaEventId.CreateFromString(id);
        var eventTitle = ViaEventTitle.Create(title);
        var combinedResult = OperationResult<ViaEventUpdateTitleCommand>.Combine(eventId.OperationErrors, eventTitle.OperationErrors);
        return combinedResult.OperationErrors.Count>0 ? combinedResult : new ViaEventUpdateTitleCommand(eventId.Payload, eventTitle.Payload);
    }
    
    private ViaEventUpdateTitleCommand(ViaEventId id, ViaEventTitle title) => (Id, Title) = (id, title);
}