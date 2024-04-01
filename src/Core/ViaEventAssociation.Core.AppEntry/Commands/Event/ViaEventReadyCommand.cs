using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventReadyCommand
{
    public ViaEventId Id { get; }

    public static OperationResult<ViaEventReadyCommand> Create(string id)
    {
        var eventId = ViaEventId.CreateFromString(id);
        return eventId.IsFailure
            ? OperationResult<ViaEventReadyCommand>.Failure(eventId.OperationErrors)
            : OperationResult<ViaEventReadyCommand>.Success(new ViaEventReadyCommand(eventId.Payload));
    }

    private ViaEventReadyCommand(ViaEventId id) => Id = id;
}