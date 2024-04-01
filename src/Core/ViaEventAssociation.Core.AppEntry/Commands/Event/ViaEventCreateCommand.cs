using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventCreateCommand
{
    public ViaEventId Id { get; }

    public static OperationResult<ViaEventCreateCommand> Create()
    {
        var eventId = ViaEventId.Create();
        return eventId.IsFailure
            ? OperationResult<ViaEventCreateCommand>.Failure(eventId.OperationErrors)
            : OperationResult<ViaEventCreateCommand>.Success(new ViaEventCreateCommand(eventId.Payload));
    }

    public static OperationResult<ViaEventCreateCommand> Create(string id)
    {
        var eventId = ViaEventId.CreateFromString(id);
        return eventId.IsFailure
            ? OperationResult<ViaEventCreateCommand>.Failure(eventId.OperationErrors)
            : OperationResult<ViaEventCreateCommand>.Success(new ViaEventCreateCommand(eventId.Payload));
    }

    private ViaEventCreateCommand(ViaEventId id) => Id = id;
}