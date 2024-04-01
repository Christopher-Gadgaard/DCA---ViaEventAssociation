
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.CreateEvent;

public class ViaEventCreateCommandTests
{
    [Fact]
    public void ViaEventCreationCommand_GivenNothing_Success()
    {
        // Arrange
        var result = ViaEventCreateCommand.Create();
        var command = result.Payload;
        
        // Act
        Assert.True(result.IsSuccess);
        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
    }
    
    [Fact]
    public void ViaEventCreationCommand_GivenId_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var result = ViaEventCreateCommand.Create(id);
        var command = result.Payload;
        
        // Act
        Assert.True(result.IsSuccess);
        
        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());
    }
}