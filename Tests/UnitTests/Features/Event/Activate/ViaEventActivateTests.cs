using UnitTests.Common.Utilities;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;

namespace UnitTests.Features.Event.Activate;

public abstract class ViaEventActivateTests
{
    public class S1
    {
        [Fact]
        public void ActivateEvent_Success_WhenEventIsDraftAndComplete()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Some Title").WithValidPastDateTimeRange()
                .Build();
            var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-1));
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            
            // Act
            var result = viaEvent.Activate(timeProvider);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        }
    }
    
    public class S2
    {
        [Fact]
        public void ActivateEvent_Success_WhenEventIsActiveAndComplete()
        {
            // Arrange
            
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Some Title").WithStatus(ViaEventStatus.Ready)
                .Build();
            var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-1));
            Assert.Equal(ViaEventStatus.Ready, viaEvent.Status);
            
            // Act
            var result = viaEvent.Activate(timeProvider);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        }
    }
    
    public class S3
    {
        [Fact]
        public void ActivateEvent_Success_StaysActiveWhenEventIsActive()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Some Title").WithStatus(ViaEventStatus.Active)
                .Build();
            var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-1));
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
            
            // Act
            var result = viaEvent.Activate(timeProvider);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
        }
    }
    
    public class F1
    {
        
        [Fact]
        public void ActivateEvent_Failure_WhenEventIsDraftAndIncomplete()
        {
            // Arrange
            var timeProvider = new FakeTimeProvider(DateTime.Now);
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithValidPastDateTimeRange()
                .Build();
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            
            // Act
            var result = viaEvent.Activate(timeProvider);
            
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
        public void ActivateEvent_Failure_WhenEventIsCancelled()
        {
            // Arrange
            var viaEventId = ViaEventId.Create();
            var viaEvent = ViaEventTestDataFactory.Init(viaEventId.Payload).WithTitle("Test Title")
                .WithStatus(ViaEventStatus.Cancelled)
                .Build();
            var timeProvider = new FakeTimeProvider(viaEvent.DateTimeRange!.StartValue.AddDays(-1));
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
            
            // Act
            var result = viaEvent.Activate(timeProvider);
            
            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(ViaEventStatus.Cancelled, viaEvent.Status);
            Assert.Contains(result.OperationErrors, e => e.Code == ErrorCode.BadRequest);
            Assert.Contains(result.OperationErrors,
                error => error.Message != null &&
                         error.Message.Contains("Cancelled events cannot be readied or activated."));
        }
    }

  
}