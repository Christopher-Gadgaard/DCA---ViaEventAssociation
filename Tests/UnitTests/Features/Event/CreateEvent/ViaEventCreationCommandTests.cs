using Via.EventAssociation.Core.Domain.Common.Utilities;
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.CreateEvent;

public class ViaEventCreationCommandTests
{
    [Fact]
    public void ViaEventCreationCommand_Success()
    {
        // Arrange
        var timeProvider = new SystemTimeProvider();
        
        // Act
        var result = ViaCreateViaEventCommand.Create(timeProvider);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload.TimeProvider);
    }
}