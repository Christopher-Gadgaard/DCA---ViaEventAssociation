using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Application.Features.Guest;

internal class RegisterGuestHandler: ICommandHandler<RegisterGuestCommand>
{
    private readonly IViaGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    internal RegisterGuestHandler(IViaGuestRepository guestRepository, IUnitOfWork unitOfWork)
    {
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<OperationResult> Handle(RegisterGuestCommand command)
    {
           await _guestRepository.AddAsync(command.Guest);
          
            await _unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
    }
}