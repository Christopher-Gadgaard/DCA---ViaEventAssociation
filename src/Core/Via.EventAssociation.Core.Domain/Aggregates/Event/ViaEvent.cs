using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationRequestEntity;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event;

public class ViaEvent : AggregateRoot<ViaEventId>
{ 
    internal new ViaEventId Id => base.Id;
    internal ViaEventTitle Title { get; private set; }
    internal ViaEventDescription Description { get; private set; }
    internal ViaDateTimeRange? DateTimeRange { get; private set; }
    internal ViaMaxGuests MaxGuests { get; private set; }
    internal ViaEventStatus Status { get; private set; }
    internal ViaEventVisibility Visibility { get; private set; }
    internal IEnumerable<ViaGuestId> Guests => _guests;

    private List<ViaGuestId> _guests;
    private List<ViaInvitation> _invitations;
    private List<ViaInvitationRequest> _invitationRequests;
    
    private bool IsFull => _guests.Count >= MaxGuests.Value;
    
    private ViaEvent(
        ViaEventId id,
        ViaEventTitle title,
        ViaEventDescription description,
        ViaMaxGuests maxGuests,
        ViaEventStatus status,
        ViaEventVisibility visibility
    )
        : base(id)
    {
        Title = title;
        Description = description;
        MaxGuests = maxGuests;
        Status = status;
        Visibility = visibility;
        _guests = new List<ViaGuestId>();
        _invitations = new List<ViaInvitation>();
        _invitationRequests = new List<ViaInvitationRequest>();
    }

    public static OperationResult<ViaEvent> Create(ViaEventId id)
    {
        var titleResult = ViaEventTitle.Default();
        var descriptionResult = ViaEventDescription.Default();
        var maxGuestsResult = ViaMaxGuests.Default();
        const ViaEventStatus status = ViaEventStatus.Draft;
        const ViaEventVisibility visibility = ViaEventVisibility.Private;
        return new ViaEvent(id, titleResult, descriptionResult, maxGuestsResult, status, visibility);
    }

    public OperationResult UpdateTitle(ViaEventTitle newTitle)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        Title = newTitle;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateDescription(ViaEventDescription newDescription)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        Description = newDescription;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateDateTimeRange(ViaDateTimeRange newDateTimeRange)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        DateTimeRange = newDateTimeRange;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateStatus(ViaEventStatus newStatus)
    {
        if (newStatus == ViaEventStatus.Cancelled)
        {
            return TryCancelEvent();
        }

        switch (Status, newStatus)
        {
            case (ViaEventStatus.Draft, ViaEventStatus.Ready):
                return TryReadyEvent();

            case (ViaEventStatus.Draft, ViaEventStatus.Active):
                var readyResult = TryReadyEvent();
                return !readyResult.IsSuccess ? readyResult : TryActivateEvent();

            case (ViaEventStatus.Ready, ViaEventStatus.Active):
                return TryActivateEvent();

            case (ViaEventStatus.Active, ViaEventStatus.Active):
                return OperationResult.Success();

            default:
                return OperationResult.Failure(new List<OperationError>
                {
                    new(ErrorCode.BadRequest,
                        $"Transitioning from '{Status}' to '{newStatus}' status is not supported.")
                });
        }
    }

    public OperationResult MakePublic()
    {
        if (Status == ViaEventStatus.Cancelled)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")
            });
        }

        Visibility = ViaEventVisibility.Public;
        return OperationResult.Success();
    }

    public OperationResult MakePrivate()
    {
        if (Status is ViaEventStatus.Cancelled or ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")
            });
        }

        Visibility = ViaEventVisibility.Private;
        return OperationResult.Success();
    }

    public OperationResult Ready()
    {
        return TryReadyEvent();
    }

    public OperationResult Activate()
    {
        return TryActivateEvent();
    }

    public OperationResult SetMaxGuests(ViaMaxGuests maxGuests)
    {
        if (Status is ViaEventStatus.Cancelled)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")
            });
        }

        if (Status is ViaEventStatus.Active && maxGuests.Value < MaxGuests.Value)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Cannot reduce max guests for an active event.")
            });
        }

        MaxGuests = maxGuests;

        return OperationResult.Success();
    }

    private OperationResult TryReadyEvent()
    {
        if (Status == ViaEventStatus.Cancelled)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Cancelled events cannot be readied or activated.")
            });
        }

        if (!IsEventDataComplete(out var errorMessages))
        {
            return OperationResult.Failure(
                new List<OperationError>(errorMessages.Select(message =>
                    new OperationError(ErrorCode.BadRequest, message))));
        }

        Status = ViaEventStatus.Ready;
        return OperationResult.Success();
    }

    private OperationResult TryActivateEvent()
    {
        var readyResult = TryReadyEvent();
        
        if (readyResult.IsFailure)
        {
            return readyResult;
        }

        Status = ViaEventStatus.Active;
        return OperationResult.Success();
    }

    private OperationResult TryCancelEvent()
    {
        Status = ViaEventStatus.Cancelled;
        return OperationResult.Success();
    }

    private bool IsEventDataComplete(out List<string> errorMessages)
    {
        errorMessages = new List<string>();
        if (Title.Value == ViaEventTitle.Default().Value)
            errorMessages.Add("The title must be changed from the default.");

        if (Description is null)
            errorMessages.Add("The description must be set.");
        
        if (DateTimeRange is null)
            errorMessages.Add("The date time range must be set.");
        
        if (DateTimeRange is not null && DateTimeRange.IsPast)
            errorMessages.Add("The start time cannot be in the past.");
        
        if (MaxGuests is null)
            errorMessages.Add("The max guests must be set.");

        return errorMessages.Count == 0;
    }

    private OperationResult CheckModifiableState()
    {
        return Status is ViaEventStatus.Active or ViaEventStatus.Cancelled
            ? OperationResult.Failure(new List<OperationError>
                {new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")})
            : OperationResult.Success();
    }

    private OperationResult IfReadyRevertToDraft()
    {
        if (Status == ViaEventStatus.Ready)
        {
            Status = ViaEventStatus.Draft;
        }

        return OperationResult.Success();
    }

    private static DateTime AdjustStartTimeBasedOnBusinessRules(DateTime currentTime)
    {
        var targetStartTime = currentTime;
        if (currentTime.Hour < 8)
        {
            // Set to today at 08:00 if before 08:00 AM
            targetStartTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, 8, 0, 0);
        }
        else if (currentTime.Hour >= 1 && currentTime.AddSeconds(30).Day > currentTime.Day)
        {
            // Set to next day at 08:00 if after 01:00 AM
            targetStartTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day).AddDays(1).AddHours(8);
        }

        return targetStartTime;
    }

    public OperationResult AddParticipant(ViaGuestId guestId)
    {
        if (DateTimeRange.IsPast)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Cannot add participation too past events.")
            });
        }

        if (Status != ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
                {new OperationError(ErrorCode.BadRequest, "Participants can only be added to active events.")});
        }

        if (Visibility != ViaEventVisibility.Public)
        {
            return OperationResult.Failure(new List<OperationError>
                {new OperationError(ErrorCode.BadRequest, "Participants can only be added to public events.")});
        }

        if (IsFull)
        {
            return OperationResult.Failure(new List<OperationError>
                {new OperationError(ErrorCode.Conflict, "The event is full.")});
        }

        if (IsParticipant(guestId))
        {
            return OperationResult.Failure(new List<OperationError>
                {new OperationError(ErrorCode.BadRequest, "The guest is already a participant.")});
        }

        _guests.Add(guestId);
        return OperationResult.Success();
    }

    public OperationResult RemoveParticipant(ViaGuestId guestId)
    {
        if (!IsParticipant(guestId))
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.NotFound, "The guest is not a participant of this event.")
            });
        }

        if (DateTimeRange.StartValue <= DateTime.Now)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Cannot remove participation from ongoing or past events.")
            });
        }


        _guests.Remove(guestId);
        return OperationResult.Success();
    }

    public bool IsParticipant(ViaGuestId guestId)
    {
        return _guests.Contains(guestId);
    }

    public OperationResult SendInvitation(ViaInvitation viaInvitation)
    {
        if (IsFull)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is full.")
            });
        }

        if (Status != ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is not in an active state.")
            });
        }

        if (Visibility != ViaEventVisibility.Private)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is not private.")
            });
        }

        _invitations.Add(viaInvitation);
        return OperationResult.Success();
    }

    public OperationResult AcceptInvitation(ViaInvitationId viaInvitationId)
    {
        if (IsFull)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is full.")
            });
        }

        if (Status != ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is not in an active state.")
            });
        }

        if (Visibility != ViaEventVisibility.Private)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is not private.")
            });
        }

        ViaInvitation? viaInvitation = _invitations.FirstOrDefault(invitation => invitation.Id == viaInvitationId);
        if (viaInvitation == null)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.NotFound, "Invitation not found.")
            });
        }

        var result = viaInvitation.Accept();
        return result;
    }

    public OperationResult DeclineInvitation(ViaInvitationId viaInvitationId)
    {
        var viaInvitation = _invitations.FirstOrDefault(invitation => invitation.Id == viaInvitationId);
        if (viaInvitation == null)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.NotFound, "Invitation not found.")
            });
        }

        var result = viaInvitation.Reject();
        return result;
    }
}