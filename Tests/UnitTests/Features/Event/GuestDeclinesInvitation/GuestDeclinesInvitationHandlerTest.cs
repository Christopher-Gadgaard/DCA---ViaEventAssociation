using UnitTests.Common.Factories;
using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.GuestDeclinesInvitation;

public class GuestDeclinesInvitationHandlerTest
{
    [Fact]
    public async void GivenNothing_CreatingHandlerShouldSucceed()
    {
        var unitOfWork = new FakeUnitOfWork();
        var guestRepository = new FakeGuestRepository();
        var eventRepository = new FakeEventRepository();
        var invitationRepository = new FakeInvitationRepository();
        
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();
        eventRepository.AddEvent(viaEvent);
        guestRepository.AddGuest(guest);
        Assert.True(invitation.Status==ViaInvitationStatus.Pending);
        
        var command = GuestDeclinesInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(),
            invitation.Id.Value.ToString());
        Assert.True(command.IsSuccess);
        
        var handler = new GuestDeclinesInvitationHandler(eventRepository, guestRepository, unitOfWork, invitationRepository);
        
        var result = await handler.Handle(command.Payload);
        Assert.True(result.IsSuccess);
       Assert.True(invitation.Status==ViaInvitationStatus.Rejected);
       Assert.Contains(viaEvent.Invitations, x => x.Id == invitation.Id);
       
    }

    [Fact]
    public async void GivenNothing_CreatingHandlerShouldFail()
    {
        var unitOfWork = new FakeUnitOfWork();
        var guestRepository = new FakeGuestRepository();
        var eventRepository = new FakeEventRepository();
        var invitationRepository = new FakeInvitationRepository();

        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { })
            .WithVisibility(ViaEventVisibility.Private).Build();
        eventRepository.AddEvent(viaEvent);

        Assert.True(invitation.Status == ViaInvitationStatus.Pending);

        var command = GuestDeclinesInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(),
            invitation.Id.Value.ToString());
        Assert.True(command.IsSuccess);

        var handler =
            new GuestDeclinesInvitationHandler(eventRepository, guestRepository, unitOfWork, invitationRepository);

        var result = await handler.Handle(command.Payload);
        Assert.False(result.IsSuccess);
    }
    
    
}