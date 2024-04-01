using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.Ready;

public class ViaEventReadyCommandTests
{
    [Fact]
    public void ViaEventReadyCommand_GivenValidId_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        
        // Act
        var result = ViaEventReadyCommand.Create(id);
        var command = result.Payload;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());
    }
}