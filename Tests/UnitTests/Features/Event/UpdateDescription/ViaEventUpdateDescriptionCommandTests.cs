using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.UpdateDescription;

public class ViaEventUpdateDescriptionCommandTests
{
    [Fact]
    public void ViaEventUpdateDescriptionCommand_GivenValidIdAndDescription_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string description = "Test Description";

        // Act
        var result = ViaEventUpdateDescriptionCommand.Create(id, description);
        var command = result.Payload;

        // Assert
        Assert.True(result.IsSuccess);

        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());

        Assert.NotNull(command.Description);
        Assert.NotNull(command.Description.ToString());
        Assert.NotEmpty(command.Description.ToString()!);
        Assert.Equal(description, command.Description.Value);
    }
}