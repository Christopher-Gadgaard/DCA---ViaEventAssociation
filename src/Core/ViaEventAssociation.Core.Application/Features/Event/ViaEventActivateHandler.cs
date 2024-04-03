using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Application.Features.Event;

public class ViaEventActivateHandler : ICommandHandler<ViaEventActivateCommand>
{
    private readonly IViaEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITimeProvider _timeProvider;
    
    internal ViaEventActivateHandler(IViaEventRepository eventRepository, IUnitOfWork unitOfWork, ITimeProvider timeProvider) =>
        (_eventRepository, _unitOfWork, _timeProvider) = (eventRepository, unitOfWork, timeProvider);
    public async Task<OperationResult> HandleAsync(ViaEventActivateCommand command)
    {
        var viaEvent = await _eventRepository.GetByIdAsync(command.Id);
        var result = viaEvent.Activate(_timeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync();
        return OperationResult.Success();
    }
}