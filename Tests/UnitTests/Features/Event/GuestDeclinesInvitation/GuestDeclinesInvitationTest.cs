using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event.GuestDeclinesInvitation;

public class GuestDeclinesInvitationTest
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

        var result = viaEvent.DeclineInvitation(invitation.Id);
        
        Assert.True(result.IsSuccess);
        Assert.True(viaEvent.Invitations.Contains(invitation));
        Assert.True(invitation.Status==ViaInvitationStatus.Rejected);
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

        //decline wrong invitation
        var result = viaEvent.DeclineInvitation(ViaInvitationId.Create().Payload);
        
        Assert.False(result.IsSuccess);
        Assert.True(viaEvent.Invitations.Contains(invitation));
        Assert.True(invitation.Status==ViaInvitationStatus.Pending);
    }

    [Fact]
    public void GivenNothing_CreatingCommandShouldFailEmptyInvitationId()
    {
        var eventId = ViaEventId.Create().Payload;
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var invitation = ViaInvitationFactory.CreateValidViaInvitation(eventId, guest.Id);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithInvitations(new List<ViaInvitation> { })
            .WithVisibility(ViaEventVisibility.Private).Build();

        var result = viaEvent.DeclineInvitation(invitation.Id);

        Assert.False(result.IsSuccess);
        Assert.False(viaEvent.Invitations.Contains(invitation));
        Assert.True(invitation.Status == ViaInvitationStatus.Pending);
    }
}