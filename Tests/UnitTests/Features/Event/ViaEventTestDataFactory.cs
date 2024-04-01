using UnitTests.Common.Factories;
using UnitTests.Common.Utilities;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Utilities;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;

namespace UnitTests.Features.Event;

public class ViaEventTestDataFactory
{
    private ViaEvent _event;
    private static ITimeProvider _timeProvider;

    public static ViaEventTestDataFactory Init(ViaEventId id)
    {
        return new ViaEventTestDataFactory(id, new SystemTimeProvider());
    }

    private ViaEventTestDataFactory(ViaEventId id, ITimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
        _event = ViaEvent.Create(id).Payload;
    }


    public ViaEventTestDataFactory WithStatus(ViaEventStatus status)
    {
        SetDateIfNull();

        if (status == ViaEventStatus.Active)
        {
            WithTitle("test title");

            _event.UpdateStatus(ViaEventStatus.Ready);
            _event.UpdateStatus(status);
        }
        else
        {
            _event.UpdateStatus(status);
        }

        return this;
    }

    private void SetDateIfNull()
    {
        if (_event.DateTimeRange is not null) return;
        var validDateRange = ViaDateTimeRangeTestDataFactory.CreateValidDateRange();
        var fakeTimeProvider = new FakeTimeProvider(validDateRange.start.AddDays(-1));
        var dateTimeRangeResult =
            ViaDateTimeRange.Create(validDateRange.start, validDateRange.end, fakeTimeProvider);
        if (dateTimeRangeResult.IsSuccess)
        {
            _event.UpdateDateTimeRange(dateTimeRangeResult.Payload!);
        }
    }

    public ViaEventTestDataFactory WithTitle(string title)
    {
        var titleResult = ViaEventTitle.Create(title);
        if (titleResult.IsSuccess)
        {
            _event.UpdateTitle(titleResult.Payload!);
        }

        return this;
    }

    public ViaEventTestDataFactory WithDescription(string description)
    {
        var descriptionResult = ViaEventDescription.Create(description);
        if (descriptionResult.IsSuccess)
        {
            _event.UpdateDescription(descriptionResult.Payload!);
        }

        return this;
    }
    
    public ViaEventTestDataFactory WithValidPastDateTimeRange()
    {
        var validDateRange = ViaDateTimeRangeTestDataFactory.CreateValidDateRange();
        var fakeTimeProvider = new FakeTimeProvider(validDateRange.start.AddDays(-1));
        var dateTimeRangeResult = ViaDateTimeRange.Create(validDateRange.start, validDateRange.end, fakeTimeProvider);
        if (dateTimeRangeResult.IsSuccess)
        {
            _event.UpdateDateTimeRange(dateTimeRangeResult.Payload!);
        }

        return this;
    }

    public ViaEventTestDataFactory WithDateTimeRange(DateTime start, DateTime end)
    {
        var dateTimeRangeResult = ViaDateTimeRange.Create(start, end, _timeProvider);
        if (dateTimeRangeResult.IsSuccess)
        {
            _event.UpdateDateTimeRange(dateTimeRangeResult.Payload!);
        }

        return this;
    }

    public ViaEventTestDataFactory WithDateTimeRange(ViaDateTimeRange dateTimeRange)
    {
        _event.UpdateDateTimeRange(dateTimeRange);
        return this;
    }

    public ViaEventTestDataFactory WithMaxGuests(int maxGuests)
    {
        var maxGuestsResult = ViaMaxGuests.Create(maxGuests);
        if (maxGuestsResult.IsSuccess)
        {
            _event.SetMaxGuests(maxGuestsResult.Payload!);
        }

        return this;
    }

    public ViaEventTestDataFactory WithVisibility(ViaEventVisibility visibility)
    {
        
        if (visibility == ViaEventVisibility.Public)
        {
            _event.MakePublic();
        }
        else
        {
            _event.MakePrivate();
        }

        return this;
    }

    public ViaEventTestDataFactory WithGuests(List<ViaGuestId> guestIds)
    {
        foreach (var guestId in guestIds)
        {
            var result = _event.AddParticipant(guestId);
        }

        return this;
    }

    public ViaEvent Build()
    {
        return _event;
    }
}