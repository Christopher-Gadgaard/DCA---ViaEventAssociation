using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaCreateViaEventCommand
{
    internal readonly ITimeProvider TimeProvider;
    private ViaCreateViaEventCommand(ITimeProvider timeProvider) => TimeProvider = timeProvider;

    public static OperationResult<ViaCreateViaEventCommand> Create(ITimeProvider timeProvider)
    {
        var eventId = ViaEventId.Create();

        if (eventId.IsFailure)
        {
            return eventId.OperationErrors;
        }

        var viaEvent = ViaEvent.Create(eventId.Payload, timeProvider);

        if (viaEvent.IsFailure)
        {
            return viaEvent.OperationErrors;
        }

        return new ViaCreateViaEventCommand(timeProvider);
    }
}