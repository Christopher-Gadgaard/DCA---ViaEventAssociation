using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event.GuestAcceptsInvitation;

public class GuestAcceptsInvitationTest
{
    [Fact]
    public void AcceptingInvitationShouldSucceed()
    {
        //Arrange
        var eventId = ViaEventId.Create().Payload; var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation=ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active).WithInvitations(new List<ViaInvitation>{invitation})
            .WithVisibility(ViaEventVisibility.Private).Build();

       
        
        //Act
       var result=viaEvent.AcceptInvitation(invitation.Id);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.True(viaEvent.Invitations.Contains(invitation));
        Assert.True(invitation.Status==ViaInvitationStatus.Accepted);

    }
    
    [Fact]
    public void AcceptingInvitationShouldFail()
    {
        //Arrange
        var eventId = ViaEventId.Create().Payload; 
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation=ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active).WithInvitations(new List<ViaInvitation>{invitation})
            .WithVisibility(ViaEventVisibility.Private).Build();

       
        
        //Act
        //accept wrong invitation
       var result=viaEvent.AcceptInvitation(ViaInvitationId.Create().Payload);
        
        //Assert
        Assert.False(result.IsSuccess);
        Assert.True(viaEvent.Invitations.Contains(invitation));
        Assert.True(invitation.Status==ViaInvitationStatus.Pending);

    }
    
    
}