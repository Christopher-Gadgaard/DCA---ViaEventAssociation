using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventMakePublicCommand
{
    public ViaEventId Id { get; }

    public static OperationResult<ViaEventMakePublicCommand> Create(string id)
    {
        var eventId = ViaEventId.CreateFromString(id);
        return eventId.IsFailure
            ? OperationResult<ViaEventMakePublicCommand>.Failure(eventId.OperationErrors)
            : OperationResult<ViaEventMakePublicCommand>.Success(new ViaEventMakePublicCommand(eventId.Payload));
    }

    private ViaEventMakePublicCommand(ViaEventId id) => Id = id;
}