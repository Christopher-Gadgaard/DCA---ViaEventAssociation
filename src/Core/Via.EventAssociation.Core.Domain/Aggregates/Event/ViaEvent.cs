using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationRequestEntity;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
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
    internal IEnumerable<ViaInvitation> Invitations => _invitations;

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
    
    private ViaEvent()
    {}
  
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

    public OperationResult Ready(ITimeProvider timeProvider)
    {
        if (Status == ViaEventStatus.Cancelled)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Cancelled events cannot be readied or activated.")
            });
        }

        if (!IsEventDataComplete(out var errorMessages,timeProvider))
        {
            return OperationResult.Failure(
                new List<OperationError>(errorMessages.Select(message =>
                    new OperationError(ErrorCode.BadRequest, message))));
        }

        Status = ViaEventStatus.Ready;
        return OperationResult.Success();
    }

    public OperationResult Activate(ITimeProvider timeProvider)
    {
        var readyResult = Ready(timeProvider);
        
        if (readyResult.IsFailure)
        {
            return readyResult;
        }

        Status = ViaEventStatus.Active;
        return OperationResult.Success();
    }
    
    public OperationResult Cancel()
    {
        if (Status == ViaEventStatus.Cancelled)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event is already cancelled.")
            });
        }
        Status = ViaEventStatus.Cancelled;
        return OperationResult.Success();
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

    private bool IsEventDataComplete(out List<string> errorMessages, ITimeProvider timeProvider)
    {
        errorMessages = new List<string>();
        if (Title.Value == ViaEventTitle.Default().Value)
            errorMessages.Add("The title must be changed from the default.");

        if (Description is null)
            errorMessages.Add("The description must be set.");
        
        if (DateTimeRange is null)
            errorMessages.Add("The date time range must be set.");
        
        if (DateTimeRange is not null && DateTimeRange.StartValue < timeProvider.Now) 
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

    public OperationResult AddParticipant(ViaGuestId guestId, ITimeProvider timeProvider)
    {
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

        if (DateTimeRange.StartValue < timeProvider.Now)
        {
            return OperationResult.Failure(new List<OperationError>
                {new OperationError(ErrorCode.BadRequest, "Cannot add participants to past events.")});
        }

        if (IsParticipant(guestId))
        {
            return OperationResult.Failure(new List<OperationError>
                {new OperationError(ErrorCode.BadRequest, "The guest is already a participant.")});
        }

        _guests.Add(guestId);
        return OperationResult.Success();
    }

    public OperationResult RemoveParticipant(ViaGuestId guestId, ITimeProvider timeProvider)
    {
        if (!IsParticipant(guestId))
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.NotFound, "The guest is not a participant of this event.")
            });
        }

        if (DateTimeRange.StartValue < timeProvider.Now)
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