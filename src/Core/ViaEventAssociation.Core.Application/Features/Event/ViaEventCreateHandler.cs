using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Application.Features.Event;

internal class ViaEventCreateHandler : ICommandHandler<ViaEventCreateCommand>
{
    private readonly IViaEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    internal ViaEventCreateHandler(IViaEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(ViaEventCreateCommand command)
    {
        var result = ViaEvent.Create(command.Id);
        
        if (result.IsFailure)
        {
            return result;
        }
        
        await _eventRepository.AddAsync(result.Payload);
        
        await _unitOfWork.SaveChangesAsync();
        
        return OperationResult.Success();
    }
}