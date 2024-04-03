using UnitTests.Common.Factories;
using UnitTests.Common.Utilities;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.Ready;

public abstract class ViaEventReadyTests
{
    public class S1
    {
        [Fact]
        public void ReadyEvent_Success_WhenEventIsDraftAndComplete()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Some Title").WithValidPastDateTimeRange()
                .Build();
            var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-1));
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);


            // Act
            var result = viaEvent.Ready(timeProvider);
            
            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
        }
    }
    
    public class F1_F4
    { 
        [Fact]
        public void ReadyEvent_Failure_WhenEventIsDraftAndIncomplete()
        {
            // Arrange
            var timeProvider = new FakeTimeProvider(DateTime.Now);
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload)
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            
            // Act
            var result = viaEvent.Ready(timeProvider);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The title must be changed from the default."));
        }
    }
    
    public class F2
    {
      [Fact]
        public void ReadyEvent_Failure_WhenEventIsCancelled()
        {
            // Arrange
            var timeProvider = new FakeTimeProvider(DateTime.Now);
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Test Title")
                .WithStatus(ViaEventStatus.Cancelled)
                .Build();
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
            
            // Act
            var result = viaEvent.Ready(timeProvider);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("Cancelled events cannot be readied or activated."));
        }
    }
    
    public class F3
    {
        [Fact]
        public void ReadyEvent_Failure_WhenEventIsInThePast() 
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var dates = ViaDateTimeRangeTestDataFactory.CreateValidDateRange();
            var fakeTimeProvider = new FakeTimeProvider( dates.start.AddDays(-1));
            var dateTimeRangeResult = ViaDateTimeRange.Create(dates.start, dates.end, fakeTimeProvider);
            var timeProvider = new FakeTimeProvider(dates.start.AddDays(1));
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Test Title")
                .WithStatus(ViaEventStatus.Draft)
                .WithDateTimeRange(dateTimeRangeResult.Payload)
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            
            // Act
            var result = viaEvent.Ready(timeProvider);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("The start time cannot be in the past."));
        }
    }
}