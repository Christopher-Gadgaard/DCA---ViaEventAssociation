using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class ViaEventUpdateTimeRangeHandler : ICommandHandler<ViaEventUpdateTimeRangeCommand>
{
    private readonly IViaEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    internal ViaEventUpdateTimeRangeHandler(IViaEventRepository eventRepository, IUnitOfWork unitOfWork) =>
        (_eventRepository, _unitOfWork) = (eventRepository, unitOfWork);

    public async Task<OperationResult> HandleAsync(ViaEventUpdateTimeRangeCommand command)
    {
        var viaEvent = await _eventRepository.GetByIdAsync(command.Id);
        var result = viaEvent.UpdateDateTimeRange(command.DateTimeRange);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync();
        return OperationResult.Success();
    }
}