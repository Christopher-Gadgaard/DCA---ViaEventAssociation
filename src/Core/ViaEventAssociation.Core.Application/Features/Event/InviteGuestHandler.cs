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

internal class InviteGuestHandler : ICommandHandler<InviteGuestCommand>
{
    private readonly IViaEventRepository _eventRepository;
    private readonly IViaGuestRepository _guestRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IViaInvitationRepository _invitationRepository;

    internal InviteGuestHandler(IViaEventRepository eventRepository, IViaGuestRepository guestRepository,
        IUnitOfWork unitOfWork, IViaInvitationRepository invitationRepository)
    {
        _eventRepository = eventRepository;
        _guestRepository = guestRepository;
        _unitOfWork = unitOfWork;
        _invitationRepository = invitationRepository;
    }

    public async Task<OperationResult> HandleAsync(InviteGuestCommand command)
    {
        ViaEvent? viaEvent = await _eventRepository.GetAsync(command.EventId);
        if (viaEvent == null)
        {
            return OperationResult.Failure(new List<OperationError> { new(ErrorCode.NotFound, "Event not found") });
        }

        ViaGuest? viaGuest = await _guestRepository.GetAsync(command.GuestId);
        if (viaGuest == null)
        { 
            return OperationResult.Failure(new List<OperationError> { new(ErrorCode.NotFound, "Guest not found") });
        }

        ViaInvitation viaInvitation =
            ViaInvitation.Create(ViaInvitationId.Create().Payload, viaEvent.Id, viaGuest.Id).Payload;


        await _invitationRepository.AddAsync(viaInvitation);

        OperationResult result = viaEvent.SendInvitation(viaInvitation);
        if (result.IsFailure)
        {
            return OperationResult.Failure(result.OperationErrors);
        }

        await _eventRepository.AddAsync(viaEvent);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }
}