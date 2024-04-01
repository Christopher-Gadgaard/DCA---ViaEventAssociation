using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.MakePrivate;

public class ViaEventMakePrivateCommandTests
{
    [Fact]
    public void ViaEventMakePrivateCommand_GivenValidId_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        
        // Act
        var result = ViaEventMakePrivateCommand.Create(id);
        var command = result.Payload;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());
    }
}