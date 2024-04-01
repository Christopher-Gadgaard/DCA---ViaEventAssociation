using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.GuestDeclinesInvitation;

public class GuestDeclinesInvitationCommandTest
{
    [Fact]
    public void GivenNothing_CreatingCommandShouldSucceed()
    {
        //Arrange
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();
        
        //Act
        var command = GuestDeclinesInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(),
            invitation.Id.Value.ToString());
        
        //Assert
        Assert.True(command.IsSuccess);
        Assert.Equal(eventId.Value, command.Payload.EventId.Value);
        Assert.Equal(guest.Id, command.Payload.GuestId);
        
    }
    
    [Fact]
    public void GivenNothing_CreatingCommandShouldFail()
    {
        //Arrange
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();
        
        //Act
        var command = GuestDeclinesInvitationCommand.Create(eventId.Value.ToString(), guest.Id.Value.ToString(), "");
        
        //Assert
        Assert.False(command.IsSuccess);
        
    }
    [Fact]
    public void GivenNothing_CreatingCommandShouldFailEmptyGuestId()
    {
        //Arrange
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();
        
        //Act
        var command =
            GuestDeclinesInvitationCommand.Create(eventId.Value.ToString(), "", invitation.Id.Value.ToString());
        
        //Assert
        Assert.False(command.IsSuccess);
        
    }
    [Fact]
    public void GivenNothing_CreatingCommandShouldFailEmptyEventId()
    {
        //Arrange
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { invitation })
            .WithVisibility(ViaEventVisibility.Private).Build();
        
        //Act
        var command = GuestDeclinesInvitationCommand.Create("", guest.Id.Value.ToString(), invitation.Id.Value.ToString());
        
        //Assert
        Assert.False(command.IsSuccess);
        
    }
}