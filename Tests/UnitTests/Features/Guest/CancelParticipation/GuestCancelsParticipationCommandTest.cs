using UnitTests.Common.Factories;
using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Guest.CancelParticipation;

public class GuestCancelsParticipationCommandTest
{
    [Fact]
    public void GivenNothing_WhenCreatingCommand_Success()
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();

        var result = GuestCancelsParticipationCommand.Create(viaEvent.Id.Value.ToString(), guest.Id.Value.ToString());

        Assert.True(result.IsSuccess);
        Assert.Equal(viaEvent.Id, result.Payload.EventId);
        Assert.Equal(guest.Id, result.Payload.GuestId);
    }
    
    [Fact]
    public void GivenValidEventId_WhenCreatingCommand_Fail()
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();

        var result = GuestCancelsParticipationCommand.Create("invalid", "");

        Assert.False(result.IsSuccess);
    }
}