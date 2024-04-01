using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventActivateCommand
{
    public ViaEventId Id { get; }

    public static OperationResult<ViaEventActivateCommand> Create(string id)
    {
        var eventId = ViaEventId.CreateFromString(id);
        return eventId.IsFailure
            ? OperationResult<ViaEventActivateCommand>.Failure(eventId.OperationErrors)
            : OperationResult<ViaEventActivateCommand>.Success(new ViaEventActivateCommand(eventId.Payload));
    }

    private ViaEventActivateCommand(ViaEventId id) => Id = id;
}