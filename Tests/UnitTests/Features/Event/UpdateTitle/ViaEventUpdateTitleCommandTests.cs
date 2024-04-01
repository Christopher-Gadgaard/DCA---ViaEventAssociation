using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.UpdateTitle;

public class ViaEventUpdateTitleCommandTests
{
    [Fact]
    public void ViaEventUpdateTitleCommand_GivenValidIdAndTitle_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string title = "Test Title";
        
        // Act
        var result = ViaEventUpdateTitleCommand.Create(id, title);
        var command = result.Payload;
        
        // Assert
        Assert.True(result.IsSuccess);
        
        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());
        
        Assert.NotNull(command.Title);
        Assert.NotNull(command.Title.ToString());
        Assert.NotEmpty(command.Title.ToString()!);
        Assert.Equal(title, command.Title.Value);
    }
}