using System.Net.NetworkInformation;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Application.Features.Event;

internal class GuestParticipateHandler:ICommandHandler<GuestParticipateCommand>
{
    private readonly IViaEventRepository _eventRepository;
    private readonly IViaGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITimeProvider _timeProvider;
    
    internal GuestParticipateHandler(IViaEventRepository eventRepository, IViaGuestRepository guestRepository, IUnitOfWork unitOfWork, ITimeProvider timeProvider)
    {
        _eventRepository = eventRepository;
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }
    
    public async Task<OperationResult> Handle(GuestParticipateCommand command)
    {
        ViaEvent? viaEvent= await _eventRepository.GetByIdAsync(command.EventId);
        if(viaEvent == null)
        {
            return OperationResult.Failure(new List<OperationError>{new(ErrorCode.NotFound, "Event not found")});
        }
        
        ViaGuest? viaGuest = await _guestRepository.GetByIdAsync(command.GuestId);
        if(viaGuest == null)
        {
            return OperationResult.Failure(new List<OperationError>{new(ErrorCode.NotFound, "Guest not found")});
        }

        OperationResult result = viaEvent.AddParticipant(viaGuest.Id, _timeProvider);
        if(result.IsFailure)
        {
            return OperationResult.Failure(result.OperationErrors);
        }
        if(result.IsSuccess)
        {
            await _eventRepository.UpdateAsync(viaEvent);
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}