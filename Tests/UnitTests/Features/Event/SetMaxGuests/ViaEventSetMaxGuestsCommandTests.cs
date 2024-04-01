using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.SetMaxGuests;

public class ViaEventSetMaxGuestsCommandTests
{
    [Fact]
    public void ViaEventSetMaxGuestsCommand_GivenValidIdAndMaxGuests_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const int maxGuests = 10;

        // Act
        var result = ViaEventSetMaxGuestsCommand.Create(id, maxGuests);
        var command = result.Payload;

        // Assert
        Assert.True(result.IsSuccess);

        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());

        Assert.NotNull(command.MaxGuests);
        Assert.Equal(maxGuests, command.MaxGuests.Value);
    }
}