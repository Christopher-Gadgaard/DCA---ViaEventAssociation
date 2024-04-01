using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class ViaEventUpdateTimeRangeCommand
{
    public ViaEventId Id { get; }
    public ViaDateTimeRange DateTimeRange { get; }
    
    public static OperationResult<ViaEventUpdateTimeRangeCommand> Create(string id, DateTime start, DateTime end, ITimeProvider timeProvider)
    {
        var eventId = ViaEventId.CreateFromString(id);
        var dateTimeRange = ViaDateTimeRange.Create(start, end, timeProvider);
        var combinedResult = OperationResult<ViaEventUpdateTimeRangeCommand>.Combine(eventId.OperationErrors, dateTimeRange.OperationErrors);
        return combinedResult.OperationErrors.Count>0 ? combinedResult : new ViaEventUpdateTimeRangeCommand(eventId.Payload, dateTimeRange.Payload);
    }
    
    private ViaEventUpdateTimeRangeCommand(ViaEventId id, ViaDateTimeRange dateTimeRange) => (Id, DateTimeRange) = (id, dateTimeRange);
}