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

    public static ViaEventTestDataFactory Init(ViaEventId id)
    {
        return new ViaEventTestDataFactory(id);
    }

    private ViaEventTestDataFactory(ViaEventId id)
    {
        var systemTime = new SystemTimeProvider();
        _event = ViaEvent.Create(id,systemTime).Payload;
    }


    public ViaEventTestDataFactory WithStatus(ViaEventStatus status)
    {
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

    public ViaEventTestDataFactory WithDateTimeRange(DateTime start, DateTime end)
    {
        var fakeTimeProvider = new FakeTimeProvider(start.AddDays(-1));
        var dateTimeRangeResult = ViaDateTimeRange.Create(start, end, fakeTimeProvider);
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
            
           var result= _event.AddParticipant(guestId);
        }

        return this;
    }

    public ViaEvent Build()
    {
        return _event;
    }
}