using UnitTests.Common.Factories;
using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.InviteGuest;

public class InviteGuestHandlerTest
{
    [Fact]
    public async void GivenNothing_CreatingHandlerShouldSucceed()
    {
        var _fakeUnitOfWork = new FakeUnitOfWork();
        var guestRepository = new FakeGuestRepository();
        var eventRepository = new FakeEventRepository();
        var invitationRepository = new FakeInvitationRepository();

        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithVisibility(ViaEventVisibility.Private)
            .WithStatus(ViaEventStatus.Active).Build();
        eventRepository.AddEvent(viaEvent);
        var guest = ViaGuestTestFactory.CreateValidViaGuest();

        var command = InviteGuestCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString());


        Assert.True(command.IsSuccess);
        var handler = new InviteGuestHandler(eventRepository, guestRepository, _fakeUnitOfWork, invitationRepository);
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        var result = await handler.Handle(command.Payload);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async void GivenNothing_CreatingHandlerShouldFail()
    {
        var _fakeUnitOfWork = new FakeUnitOfWork();
        var guestRepository = new FakeGuestRepository();
        var eventRepository = new FakeEventRepository();
        var invitationRepository = new FakeInvitationRepository();

        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithVisibility(ViaEventVisibility.Public)
            .WithStatus(ViaEventStatus.Active).Build();
        eventRepository.AddEvent(viaEvent);
        var guest = ViaGuestTestFactory.CreateValidViaGuest();

        var command = InviteGuestCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString());


        Assert.True(command.IsSuccess);
        var handler = new InviteGuestHandler(eventRepository, guestRepository, _fakeUnitOfWork, invitationRepository);
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        var result = await handler.Handle(command.Payload);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async void GivenNothing_CreatingHandlerShouldFailWhenDraft()
    {
        var _fakeUnitOfWork = new FakeUnitOfWork();
        var guestRepository = new FakeGuestRepository();
        var eventRepository = new FakeEventRepository();
        var invitationRepository = new FakeInvitationRepository();

        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithVisibility(ViaEventVisibility.Private)
            .WithStatus(ViaEventStatus.Draft).Build();
        eventRepository.AddEvent(viaEvent);
        var guest = ViaGuestTestFactory.CreateValidViaGuest();

        var command = InviteGuestCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString());


        Assert.True(command.IsSuccess);
        var handler = new InviteGuestHandler(eventRepository, guestRepository, _fakeUnitOfWork, invitationRepository);
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        var result = await handler.Handle(command.Payload);

        Assert.False(result.IsSuccess);
    }
}