using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventMakePrivateCommand
{
    public ViaEventId Id { get; }

    public static OperationResult<ViaEventMakePrivateCommand> Create(string id)
    {
        var eventId = ViaEventId.CreateFromString(id);
        return eventId.IsFailure
            ? OperationResult<ViaEventMakePrivateCommand>.Failure(eventId.OperationErrors)
            : OperationResult<ViaEventMakePrivateCommand>.Success(new ViaEventMakePrivateCommand(eventId.Payload));
    }

    private ViaEventMakePrivateCommand(ViaEventId id) => Id = id;
}