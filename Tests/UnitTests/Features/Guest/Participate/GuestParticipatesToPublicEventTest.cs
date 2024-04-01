
using UnitTests.Common.Factories;
using UnitTests.Common.Utilities;
using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Guest.Participate;

public class GuestParticipatesToPublicEventTest
{
    [Fact]
    public void Guest_Participation_Successful()
    {
        // Arrange
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).Build();

        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);

        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        Assert.Equal("308826@via.dk", viaGuest.ViaEmail.Value);
        Assert.NotNull(viaGuest.Id);

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(viaEvent.IsParticipant(viaGuest.Id));
    }

    [Fact]
    public void GuestParticipationFailsInDraftState()
    {
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithVisibility(ViaEventVisibility.Public)
            .WithStatus(ViaEventStatus.Draft).Build();

        Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);

        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        Assert.Equal("308826@via.dk", viaGuest.ViaEmail.Value);
        Assert.NotNull(viaGuest.Id);

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }

    [Fact]
    public void GuestParticipationFailsWhenPrivateEvent()
    {
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithVisibility(ViaEventVisibility.Private)
            .WithStatus(ViaEventStatus.Active).Build();

        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        Assert.Equal("308826@via.dk", viaGuest.ViaEmail.Value);
        Assert.NotNull(viaGuest.Id);

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }

    [Fact]
    public void AddingGuestFailsWhenAlreadyAParticipant()
    {
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithVisibility(ViaEventVisibility.Public)
            .WithStatus(ViaEventStatus.Active).Build();

        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        Assert.Equal("308826@via.dk", viaGuest.ViaEmail.Value);
        Assert.NotNull(viaGuest.Id);

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id);
        Assert.True(result.IsSuccess);
        Assert.True(viaEvent.IsParticipant(viaGuest.Id));
        
        var addAgain= viaEvent.AddParticipant(viaGuest.Id);
        // Assert
     
        Assert.True(addAgain.IsFailure);
        Assert.True(viaEvent.IsParticipant(viaGuest.Id));
        Assert.Contains( addAgain.OperationErrors, error => error is { Code: ErrorCode.BadRequest, Message: "The guest is already a participant." });
    }
    
   
    [Fact]
    public void Guest_Participation_Fails_When_Event_Is_Canceled()
    {
        // Arrange
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
            .WithStatus(ViaEventStatus.Cancelled)
            .WithVisibility(ViaEventVisibility.Public)
            .Build();

        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }
    [Fact]
    public void Guest_Participation_Fails_When_Event_Is_Full()
    {
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Active) .WithVisibility(ViaEventVisibility.Public)
            .WithMaxGuests(5).WithGuests( new List<ViaGuestId>(){ViaGuestId.Create().Payload, ViaGuestId.Create().Payload, ViaGuestId.Create().Payload, ViaGuestId.Create().Payload, ViaGuestId.Create().Payload})
            .Build();

        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
        Assert.Equal(5, viaEvent.Guests.Count());
        
        
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.Conflict && error.Message == "The event is full.");
    }

    [Fact]
    public void Guest_Participation_Fails_When_Event_Start_Time_Is_In_The_Past() //TODO: fix this test
    {
        var startTime = new DateTime(2020, 1, 2, 10, 0, 0);
        var endTime = new DateTime(2020, 1, 2, 16, 0, 0);
        FakeTimeProvider fakeTimeProvider = new(new DateTime(2020, 1, 3, 10, 0, 0));
        
        var dateTimeRange = ViaDateTimeRange.Create(startTime,endTime,fakeTimeProvider);
        
        // Arrange
        var pastStartTime = DateTime.UtcNow.AddDays(-1);
        var pastEndTime = DateTime.UtcNow.AddHours(-23);
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
            .WithDateTimeRange(dateTimeRange.Payload)
            .WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public)
            .Build();

        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }
}

