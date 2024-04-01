using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event.InviteGuest;

public class InviteGuestTest
{
    [Fact]
    public void InvitingGuestShouldSucceed()
    {
        //Arrange
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Private).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation=ViaInvitationFactory.CreateValidViaInvitation(viaEvent.Id, guest.Id);
        //Act
       var result= viaEvent.SendInvitation(invitation);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.True(viaEvent.Invitations.Contains(invitation));

    }
    
    [Fact]
    public void InvitingGuestToPublicShouldFail()
    {
        //Arrange
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation=ViaInvitationFactory.CreateValidViaInvitation(viaEvent.Id, guest.Id);
        //Act
        var result =viaEvent.SendInvitation(invitation);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.False(viaEvent.Invitations.Contains(invitation));

    }
}