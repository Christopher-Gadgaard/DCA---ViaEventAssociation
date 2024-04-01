using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.InviteGuest;

public class InviteGuestCommandTest
{
    [Fact]
    public void GivenNothing_WhenCreatingCommand_Success()
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Private).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(viaEvent.Id, guest.Id);

        var result = InviteGuestCommand.Create(viaEvent.Id.Value.ToString(), guest.Id.Value.ToString());

        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void GivenValidEventId_WhenCreatingCommand_Fail()
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Private).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();

        var result = InviteGuestCommand.Create("", guest.Id.Value.ToString());

        Assert.False(result.IsSuccess);
    }
    
    [Fact]
    public void GivenValidGuestId_WhenCreatingCommand_Fail()
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Private).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();

        var result = InviteGuestCommand.Create(viaEvent.Id.Value.ToString(), "");

        Assert.False(result.IsSuccess);
    }
}