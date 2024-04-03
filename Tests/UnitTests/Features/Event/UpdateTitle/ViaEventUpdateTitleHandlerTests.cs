using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.UpdateTitle;

public class ViaEventUpdateTitleHandlerTests
{
    [Fact]
    public async Task ViaEventUpdateTitleHandler_GivenValidCommand_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string title = "Test Title";

        var command = ViaEventUpdateTitleCommand.Create(id, title).Payload;

        var eventRepository = new FakeEventRepository();
        eventRepository.AddEvent(ViaEventTestDataFactory.Init(command.Id).WithTitle(title).Build());

        var unitOfWork = new FakeUnitOfWork();

        var handler = new ViaEventUpdateTitleHandler(eventRepository, unitOfWork);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(eventRepository.Events);

        var viaEvent = eventRepository.Events.First();
        Assert.Equal(command.Id, viaEvent.Id);
        Assert.Equal(command.Title, viaEvent.Title);
    }
}