
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.CreateEvent;

public class ViaEventCreateCommandTests
{
    [Fact]
    public void ViaEventCreationCommand_GivenNothing_Success()
    {
        // Act
        var result = ViaEventCreateCommand.Create();
        var command = result.Payload;
        
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
        
        // Act
        var result = ViaEventCreateCommand.Create(id);
        var command = result.Payload;
        
        // Assert
        Assert.True(result.IsSuccess);
        
        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());
    }
    
    [Fact]
    public void ViaEventCreationCommand_GivenBadId_Failure()
    {
        // Arrange
        var id = string.Empty;
        
        // Act
        var result = ViaEventCreateCommand.Create(id);
        
        // Arrange
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid id", result.OperationErrors.First().Message);
    }
}