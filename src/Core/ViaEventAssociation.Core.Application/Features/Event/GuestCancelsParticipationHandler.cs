using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.Application.Features.Event;

internal class GuestCancelsParticipationHandler : ICommandHandler<GuestCancelsParticipationCommand>
{
    private readonly IViaGuestRepository _guestRepository;
    private readonly IViaEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITimeProvider _timeProvider;

    internal GuestCancelsParticipationHandler(IViaGuestRepository guestRepository, IViaEventRepository eventRepository,
        IUnitOfWork unitOfWork, ITimeProvider timeProvider)
    {
        _guestRepository = guestRepository;
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
        _timeProvider = timeProvider;
    }

    public async Task<OperationResult> HandleAsync(GuestCancelsParticipationCommand command)
    {
        ViaEvent? viaEvent = await _eventRepository.GetByIdAsync(command.EventId);
        if (viaEvent == null)
        {
            return OperationResult.Failure(new List<OperationError> { new(ErrorCode.NotFound, "Event not found") });
        }

        ViaGuest? viaGuest = await _guestRepository.GetByIdAsync(command.GuestId);
        if (viaGuest == null)
        {
            return OperationResult.Failure(new List<OperationError> { new(ErrorCode.NotFound, "Guest not found") });
        }

        OperationResult result = viaEvent.RemoveParticipant(viaGuest.Id, _timeProvider);
      
        if (result.IsSuccess)
        {
            await _eventRepository.UpdateAsync(viaEvent);
            await _guestRepository.UpdateAsync(viaGuest);
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}