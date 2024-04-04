using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.Activate;

public class ViaEventActivateCommandTests
{
    [Fact]
    public void ViaEventActivateCommand_GivenValidId_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        
        // Act
        var result = ViaEventActivateCommand.Create(id);
        var command = result.Payload;
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());
    }
    
    [Fact]
    public void ViaEventActivateCommand_GivenInvalidId_Failure()
    {
        // Arrange
        var id = string.Empty;
        
        // Act
        var result = ViaEventActivateCommand.Create(id);
        
        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal("Invalid id", result.OperationErrors.First().Message);
    }
}