using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.GuestAcceptsInvitation;

public class GuestAcceptsInvitationCommandTest
{
    [Fact]
    public void GivenNothing_CreatingCommandShouldSucceed()
    {
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();

        var command = GuestAcceptsInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(),
            invitation.Id.Value.ToString());

        Assert.True(command.IsSuccess);
        Assert.Equal(eventId.Value, command.Payload.EventId.Value);
        Assert.Equal(guest.Id, command.Payload.GuestId);
    }

    [Fact]
    public void GivenNothing_CreatingCommandShouldFail()
    {
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();

        var command = GuestAcceptsInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(), "");

        Assert.False(command.IsSuccess);
    }

    [Fact]
    public void GivenNothing_CreatingCommandShouldFailEmptyGuestId()
    {
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();

        var command =
            GuestAcceptsInvitationCommand.Create(eventId.Value.ToString(), "", invitation.Id.Value.ToString());

        Assert.False(command.IsSuccess);
    }

    [Fact]
    public void GivenNothing_CreatingCommandShouldFailEmptyEventId()
    {
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();

        var command = GuestAcceptsInvitationCommand.Create("", guest.Id.Value.ToString(), invitation.Id.Value.ToString());

        Assert.False(command.IsSuccess);
    }
}