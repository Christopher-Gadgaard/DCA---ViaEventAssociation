using UnitTests.Common.Factories;
using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Guest.Participate;

public class GuestParticipatesToPublicEventCommandTest
{
    [Fact]
    public void GivenNothing_WhenCreatingCommand_Success()
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();

        var result = GuestParticipateCommand.Create(viaEvent.Id.Value, guest.Id.Value);

        Assert.True(result.IsSuccess);
    }
    


}