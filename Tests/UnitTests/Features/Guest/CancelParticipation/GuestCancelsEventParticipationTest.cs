using Moq;
using UnitTests.Common.Factories;
using UnitTests.Common.Utilities;
using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Aggregates.Locations;
using Via.EventAssociation.Core.Domain.Common.Utilities;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Guest.CancelParticipation;

public class GuestCancelsEventParticipationTest
{
    [Fact]
    public void Guest_Removes_Participation_From_Public_Event_Successfully()
    {
        // Arrange
        var eventId= ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId) 
            .WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public)
            .Build();
        var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-2));
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();

        viaEvent.AddParticipant(viaGuest.Id, timeProvider);

        // Pre-Assertion
        Assert.True(viaEvent.IsParticipant(viaGuest.Id));

        // Act
        var result = viaEvent.RemoveParticipant(viaGuest.Id, timeProvider);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }
    [Fact]
    public void Guest_Removal_Does_Nothing_When_Not_A_Participant()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider(DateTime.Now);
        var eventId= ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId)
            .WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public)
            .Build();


        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();

        // Act
        var result = viaEvent.RemoveParticipant(viaGuest.Id, timeProvider);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.NotFound);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }

//    [Fact]
    /*public void Guest_Removal_Fails_For_Ongoing_Event()
    {
        // Arrange
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        var startTime = new DateTime( 2022, 08, 25, 10, 00, 00); 
        var endTime = DateTime.UtcNow.AddDays(2);
        var eventId= ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId)
            .WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).WithDateTimeRange( startTime, endTime)
            .WithGuests(new List<ViaGuestId>{viaGuest.Id}).Build();

        Assert.True(viaEvent.IsParticipant(viaGuest.Id));
        Assert.Equal( ViaEventStatus.Active, viaEvent.Status);
        // Assert.Equal(viaEvent.DateTimeRange.StartValue,  DateTime.UtcNow.AddDays(-1));

       Assert.True(viaEvent.DateTimeRange.StartValue< DateTime.Now);


        // Act
        var result = viaEvent.RemoveParticipant(viaGuest.Id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(viaEvent.IsParticipant(viaGuest.Id)); 
    }*/
}