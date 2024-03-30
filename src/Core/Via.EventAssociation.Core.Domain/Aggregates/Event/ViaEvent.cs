﻿using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Utilities;
using Via.EventAssociation.Core.Domain.Common.Utilities.Interfaces;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event;

public class ViaEvent : AggregateRoot<ViaEventId>
{
    private const string DefaultTitle = "Working Title";
    private ViaEventTitle _title;
    private ViaEventDescription _description;
    private ViaDateTimeRange _dateTimeRange;
    private ViaMaxGuests _maxGuests;
    private ViaEventStatus _status;
    private ViaEventVisibility _visibility;
    private List<ViaGuestId> _guests;
    private List<ViaInvitation> _invitations;
    private List<ViaInvitationRequest> _invitationRequests;
    private static ITimeProvider? _timeProvider;

    internal new ViaEventId Id => base.Id;
    internal ViaEventTitle? Title => _title;
    internal ViaEventDescription? Description => _description;
    internal ViaDateTimeRange? DateTimeRange => _dateTimeRange;
    internal ViaMaxGuests MaxGuests => _maxGuests;
    internal ViaEventStatus Status => _status;
    internal ViaEventVisibility Visibility => _visibility;
    internal IEnumerable<ViaGuestId> Guests => _guests;

    private ViaEvent(
        ViaEventId id,
        ITimeProvider timeProvider,
        ViaEventTitle? title = null,
        ViaEventDescription? description = null,
        ViaDateTimeRange? dateTimeRange = null,
        ViaMaxGuests? maxGuests = null,
        ViaEventStatus status = ViaEventStatus.Draft,
        ViaEventVisibility visibility = ViaEventVisibility.Private
    )
        : base(id)
    {
        _timeProvider = timeProvider;
        _title = title ?? ViaEventTitle.Create(DefaultTitle).Payload;
        _description = description ?? ViaEventDescription.Create("").Payload;
        var validStartTime = AdjustStartTimeBasedOnBusinessRules(_timeProvider.Now);
        _dateTimeRange = dateTimeRange ?? ViaDateTimeRange
            .Create(validStartTime, validStartTime.AddHours(1), _timeProvider)
            .Payload;
        _maxGuests = maxGuests ?? ViaMaxGuests.Create(5).Payload;
        _status = status;
        _visibility = visibility;
        _guests = new List<ViaGuestId>();
    }

    public static OperationResult<ViaEvent> Create(ViaEventId id, ITimeProvider timeProvider)
    {
        return new ViaEvent(id, timeProvider);
    }

    public OperationResult UpdateTitle(ViaEventTitle newTitle)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        _title = newTitle;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateDescription(ViaEventDescription newDescription)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        _description = newDescription;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateDateTimeRange(ViaDateTimeRange newDateTimeRange)
    {
        var modifiableStateCheck = CheckModifiableState();
        if (modifiableStateCheck.IsFailure)
        {
            return modifiableStateCheck;
        }

        _dateTimeRange = newDateTimeRange;

        return IfReadyRevertToDraft();
    }

    public OperationResult UpdateStatus(ViaEventStatus newStatus)
    {
        if (newStatus == ViaEventStatus.Cancelled)
        {
            return TryCancelEvent();
        }

        switch (_status, newStatus)
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
                        $"Transitioning from '{_status}' to '{newStatus}' status is not supported.")
                });
        }
    }

    public OperationResult MakePublic()
    {
        if (_status == ViaEventStatus.Cancelled)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")
            });
        }

        _visibility = ViaEventVisibility.Public;
        return OperationResult.Success();
    }

    public OperationResult MakePrivate()
    {
        if (_status is ViaEventStatus.Cancelled or ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")
            });
        }

        _visibility = ViaEventVisibility.Private;
        return OperationResult.Success();
    }

    public OperationResult SetMaxGuests(ViaMaxGuests maxGuests)
    {
        if (_status is ViaEventStatus.Cancelled)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "The event cannot be modified in its current state.")
            });
        }

        if (_status is ViaEventStatus.Active && maxGuests.Value < _maxGuests.Value)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Cannot reduce max guests for an active event.")
            });
        }

        _maxGuests = maxGuests;

        return OperationResult.Success();
    }

    private OperationResult TryReadyEvent()
    {
        if (!IsEventDataComplete(out List<string> errorMessages))
        {
            return OperationResult.Failure(
                new List<OperationError>(errorMessages.Select(message =>
                    new OperationError(ErrorCode.BadRequest, message))));
        }

        _status = ViaEventStatus.Ready;
        return OperationResult.Success();
    }

    private OperationResult TryActivateEvent()
    {
        if (_timeProvider.Now >= _dateTimeRange!.StartValue.AddSeconds(30))
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.BadRequest, "Cannot activate past events.")
            });
        }

        _status = ViaEventStatus.Active;
        return OperationResult.Success();
    }

    private OperationResult TryCancelEvent()
    {
        _status = ViaEventStatus.Cancelled;
        return OperationResult.Success();
    }

    private bool IsEventDataComplete(out List<string> errorMessages)
    {
        errorMessages = new List<string>();
        if (_title == null || _title.Value == DefaultTitle)
            errorMessages.Add("The title must be changed from the default.");

        if (_description == null)
            errorMessages.Add("The description must be set.");

        if (_dateTimeRange == null || _dateTimeRange.StartValue.AddSeconds(30) < _timeProvider.Now)
            errorMessages.Add("The start time cannot be in the past.");

        if (_maxGuests == null)
            errorMessages.Add("The max guests must be set.");

        return errorMessages.Count == 0;
    }

    private OperationResult CheckModifiableState()
    {
        return _status is ViaEventStatus.Active or ViaEventStatus.Cancelled
            ? OperationResult.Failure(new List<OperationError>
                { new(ErrorCode.BadRequest, "The event cannot be modified in its current state.") })
            : OperationResult.Success();
    }

    private OperationResult IfReadyRevertToDraft()
    {
        if (_status == ViaEventStatus.Ready)
        {
            _status = ViaEventStatus.Draft;
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
        if (_status != ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
                { new OperationError(ErrorCode.BadRequest, "Participants can only be added to active events.") });
        }

        if (_visibility != ViaEventVisibility.Public)
        {
            return OperationResult.Failure(new List<OperationError>
                { new OperationError(ErrorCode.BadRequest, "Participants can only be added to public events.") });
        }

        if (IsFull())
        {
            return OperationResult.Failure(new List<OperationError>
                { new OperationError(ErrorCode.Conflict, "The event is full.") });
        }

        if (IsParticipant(guestId))
        {
            return OperationResult.Failure(new List<OperationError>
                { new OperationError(ErrorCode.BadRequest, "The guest is already a participant.") });
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
                new OperationError(ErrorCode.NotFound, "The guest is not a participant of this event.")
            });
        }

        if (_dateTimeRange.StartValue <= DateTime.Now)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new OperationError(ErrorCode.BadRequest, "Cannot remove participation from ongoing or past events.")
            });
        }


        _guests.Remove(guestId);
        return OperationResult.Success();
    }


    public bool IsFull()
    {
        return _guests.Count >= _maxGuests.Value;
    }

    public bool IsParticipant(ViaGuestId guestId)
    {
        return _guests.Contains(guestId);
    }

    public OperationResult SendInvitation(ViaInvitation viaInvitation)
    {
        if (IsFull())
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is full.")
            });
        }

        if (_status != ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is not in an active state.")
            });
        }

        if (_visibility != ViaEventVisibility.Private)
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
        if (IsFull())
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is full.")
            });
        }

        if (_status != ViaEventStatus.Active)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.Conflict, "The event is not in an active state.")
            });
        }

        if (_visibility != ViaEventVisibility.Private)
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

        var result= viaInvitation.Accept();
        return result;
    }

    public OperationResult DeclineInvitation(ViaInvitationId viaInvitationId)
    {
        ViaInvitation? viaInvitation = _invitations.FirstOrDefault(invitation => invitation.Id == viaInvitationId);
        if (viaInvitation == null)
        {
            return OperationResult.Failure(new List<OperationError>
            {
                new(ErrorCode.NotFound, "Invitation not found.")
            });
        }

        var result= viaInvitation.Reject();
        return result;
    }
}

