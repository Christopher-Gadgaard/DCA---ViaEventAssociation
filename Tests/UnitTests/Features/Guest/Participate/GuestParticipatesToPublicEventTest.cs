
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
        var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-1));
        
        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);

        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        Assert.Equal("308826@via.dk", viaGuest.ViaEmail.Value);
        Assert.NotNull(viaGuest.Id);

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id, timeProvider);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(viaEvent.IsParticipant(viaGuest.Id));
    }

    [Fact]
    public void GuestParticipationFailsInDraftState()
    {
        var timeProvider = new FakeTimeProvider(DateTime.Now);
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithVisibility(ViaEventVisibility.Public)
            .WithStatus(ViaEventStatus.Draft).Build();

        Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);

        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        Assert.Equal("308826@via.dk", viaGuest.ViaEmail.Value);
        Assert.NotNull(viaGuest.Id);

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id, timeProvider);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }

    [Fact]
    public void GuestParticipationFailsWhenPrivateEvent()
    {
        var timeProvider = new FakeTimeProvider(DateTime.Now);
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithVisibility(ViaEventVisibility.Private)
            .WithStatus(ViaEventStatus.Active).Build();

        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        Assert.Equal("308826@via.dk", viaGuest.ViaEmail.Value);
        Assert.NotNull(viaGuest.Id);

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id, timeProvider);

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
        var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-1));
        
        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        Assert.Equal("308826@via.dk", viaGuest.ViaEmail.Value);
        Assert.NotNull(viaGuest.Id);

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id, timeProvider);
        Assert.True(result.IsSuccess);
        Assert.True(viaEvent.IsParticipant(viaGuest.Id));
        
        var addAgain= viaEvent.AddParticipant(viaGuest.Id, timeProvider);
        // Assert
     
        Assert.True(addAgain.IsFailure);
        Assert.True(viaEvent.IsParticipant(viaGuest.Id));
        Assert.Contains( addAgain.OperationErrors, error => error is { Code: ErrorCode.BadRequest, Message: "The guest is already a participant." });
    }
    
   
    [Fact]
    public void Guest_Participation_Fails_When_Event_Is_Canceled()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider(DateTime.Now);
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
            .WithStatus(ViaEventStatus.Cancelled)
            .WithVisibility(ViaEventVisibility.Public)
            .Build();

        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id, timeProvider);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }
    [Fact]
    public void Guest_Participation_Fails_When_Event_Is_Full()
    {
        var timeProvider = new FakeTimeProvider(DateTime.Now);
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithStatus(ViaEventStatus.Active) .WithVisibility(ViaEventVisibility.Public)
            .WithMaxGuests(5).WithGuests( new List<ViaGuestId>(){ViaGuestId.Create().Payload, ViaGuestId.Create().Payload, ViaGuestId.Create().Payload, ViaGuestId.Create().Payload, ViaGuestId.Create().Payload})
            .Build();

        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Public, viaEvent.Visibility);
        Assert.Equal(5, viaEvent.Guests.Count());
        
        
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id, timeProvider);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
        Assert.Contains(result.OperationErrors, error => error.Code == ErrorCode.Conflict && error.Message == "The event is full.");
    }

    [Fact]
    public void Guest_Participation_Fails_When_Event_Start_Time_Is_In_The_Past()
    {
        
        // Arrange
        var dates = ViaDateTimeRangeTestDataFactory.CreateValidDateRange();
        var fakeTimeProvider = new FakeTimeProvider( dates.start.AddDays(-1));
        var dateTimeRangeResult = ViaDateTimeRange.Create(dates.start, dates.end, fakeTimeProvider);
        var timeProvider = new FakeTimeProvider(dates.start.AddDays(1));
        var viaEventId = ViaEventId.Create();
        var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
            .WithDateTimeRange(dateTimeRangeResult.Payload)
            .WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public)
            .Build();

        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();

        // Act
        var result = viaEvent.AddParticipant(viaGuest.Id,timeProvider);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(viaEvent.IsParticipant(viaGuest.Id));
    }
}

