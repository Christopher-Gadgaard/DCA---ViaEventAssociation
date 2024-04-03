using Moq;
using UnitTests.Common.Factories;
using UnitTests.Common.Utilities;
using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Guest.CancelParticipation;

public class GuestCancelsParticipationHandlerTest
{
    [Fact]
    public async Task GivenNothing_WhenCreatingHandler_Success()
    {
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var eventId = ViaEventId.Create().Payload;
        
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).WithGuests(new List<ViaGuestId> {guest.Id})
            .Build();
        var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-1));
        Assert.Single(viaEvent.Guests);
        Assert.Equal(viaEvent.Guests.First(), guest.Id);
        Assert.True(viaEvent.IsParticipant(guest.Id));
        Assert.Contains(guest.Id, viaEvent.Guests);
        var fakeUnitOfWork = new FakeUnitOfWork();
        var guestRepo = new FakeGuestRepository();
        var eventRepo = new FakeEventRepository();

        guestRepo.AddGuest(guest);
        eventRepo.AddEvent(viaEvent);

        var command = GuestCancelsParticipationCommand.Create(viaEvent.Id.Value.ToString(), guest.Id.Value.ToString());

        Assert.True(command.IsSuccess);
        Assert.Equal(guest.Id, command.Payload.GuestId);

        var handler = new GuestCancelsParticipationHandler(guestRepo, eventRepo, fakeUnitOfWork, timeProvider);
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        var result = await handler.HandleAsync(command.Payload);

        Assert.True(result.IsSuccess);
    }
}