using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.UpdateDescription;

public class ViaEventUpdateDescriptionHandlerTests
{
    [Fact]
    public async Task ViaEventUpdateDescriptionHandler_GivenValidCommand_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string description = "Test Description";

        var command = ViaEventUpdateDescriptionCommand.Create(id, description).Payload;

        var eventRepository = new FakeEventRepository();
        eventRepository.AddEvent(ViaEventTestDataFactory.Init(command.Id).WithDescription(description).Build());

        var unitOfWork = new FakeUnitOfWork();

        var handler = new ViaEventUpdateDescriptionHandler(eventRepository, unitOfWork);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(eventRepository.Events);

        var viaEvent = eventRepository.Events.First();
        Assert.Equal(command.Id, viaEvent.Id);
        Assert.Equal(command.Description, viaEvent.Description);
    }
}