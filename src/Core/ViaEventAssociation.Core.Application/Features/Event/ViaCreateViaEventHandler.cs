using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Application.Features.Event;

internal class ViaCreateViaEventHandler : ICommandHandler<ViaCreateViaEventCommand>
{
    private readonly IViaEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    internal ViaCreateViaEventHandler(IViaEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public Task<OperationResult> Handle(ViaCreateViaEventCommand command)
    {
        /*var viaEvent = ViaEvent.Create(command.TimeProvider);
        _eventRepository.AddAsync(viaEvent);
        _unitOfWork.SaveChangesAsync();
        return OperationResult.Success();*/
        throw new NotImplementedException();
    }
}