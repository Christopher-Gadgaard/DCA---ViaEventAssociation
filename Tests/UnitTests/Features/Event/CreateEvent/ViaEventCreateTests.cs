using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Features.Event.CreateEvent;

public abstract class ViaEventCreateTests
{
    public class S1
    {
        [Fact]
        public void CreateEventWithId_Success_IsDraftHasMax5Guests()
        {
            // Arrange
            var eventId = ViaEventId.Create();
            Assert.True(eventId.IsSuccess);

            // Act
            var result = ViaEvent.Create(eventId.Payload);
            var viaEvent = result.Payload;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(eventId.Payload, viaEvent.Id);
            Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
            Assert.Equal(5, viaEvent.MaxGuests.Value);
        }
    }

    public class S2
    {
        [Fact]
        public void CreateEventWithId_Success_HasDefaultTitle()
        {
            // Arrange
            var eventId = ViaEventId.Create();
            Assert.True(eventId.IsSuccess);

            // Act
            var result = ViaEvent.Create(eventId.Payload);
            var viaEvent = result.Payload;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(eventId.Payload, viaEvent.Id);
            Assert.Equal("Working Title", viaEvent.Title.Value);
        }
    }
    
    public class S3
    {
        [Fact]
        public void CreateEventWithId_Success_DescriptionIsEmpty()
        {
            // Arrange
            var eventId = ViaEventId.Create();
            Assert.True(eventId.IsSuccess);

            // Act
            var result = ViaEvent.Create(eventId.Payload);
            var viaEvent = result.Payload;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(eventId.Payload, viaEvent.Id);
            Assert.Equal(string.Empty, viaEvent.Description.Value);
        }
    }
    
    public class S4
    {
        [Fact]
        public void CreateEventWithId_Success_IsPrivate()
        {
            // Arrange
            var eventId = ViaEventId.Create();
            Assert.True(eventId.IsSuccess);

            // Act
            var result = ViaEvent.Create(eventId.Payload);
            var viaEvent = result.Payload;

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(eventId.Payload, viaEvent.Id);
            Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
        }
    }
}