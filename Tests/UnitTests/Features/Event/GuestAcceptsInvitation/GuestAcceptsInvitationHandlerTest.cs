using UnitTests.Common.Factories;
using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.GuestAcceptsInvitation;

public class GuestAcceptsInvitationHandlerTest
{
    [Fact]
    public async void GivenNothing_CreatingHandlerShouldSucceed()
    {
        var _fakeUnitOfWork = new FakeUnitOfWork();
        var guestRepository = new FakeGuestRepository();
        var eventRepository = new FakeEventRepository();
        var invitationRepository = new FakeInvitationRepository();

        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        guestRepository.AddGuest(guest);
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();

        eventRepository.AddEvent(viaEvent);
        var command = GuestAcceptsInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(),
            invitation.Id.Value.ToString());

        Assert.True(command.IsSuccess);
        var handler =
            new GuestAcceptsInvitationHandler(eventRepository, guestRepository, _fakeUnitOfWork, invitationRepository);

        if (handler == null) throw new ArgumentNullException(nameof(handler));

        var result = await handler.HandleAsync(command.Payload);

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
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
       
        var command = GuestAcceptsInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(),
            invitation.Id.Value.ToString());

        Assert.True(command.IsSuccess);
        //event repo does not contain the event
        var handler =
            new GuestAcceptsInvitationHandler(eventRepository, guestRepository, _fakeUnitOfWork, invitationRepository);

        if (handler == null) throw new ArgumentNullException(nameof(handler));

        var result = await handler.HandleAsync(command.Payload);

        Assert.False(result.IsSuccess);
    }

    [Fact]
    public async void GivenNothing_CreatingHandlerShouldFailWhenNoInvitation()
    {
        var _fakeUnitOfWork = new FakeUnitOfWork();
        var guestRepository = new FakeGuestRepository();
        var eventRepository = new FakeEventRepository();
        var invitationRepository = new FakeInvitationRepository();

        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> {  })
            .WithVisibility(ViaEventVisibility.Private).Build();

        eventRepository.AddEvent(viaEvent);
        var command = GuestAcceptsInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(),
            invitation.Id.Value.ToString());

        Assert.True(command.IsSuccess);
        //event repo does not contain the event
        var handler =
            new GuestAcceptsInvitationHandler(eventRepository, guestRepository, _fakeUnitOfWork, invitationRepository);

        if (handler == null) throw new ArgumentNullException(nameof(handler));

        var result = await handler.HandleAsync(command.Payload);

        Assert.False(result.IsSuccess);
    }
}