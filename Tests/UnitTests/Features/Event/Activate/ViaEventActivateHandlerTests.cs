using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.Activate;

public class ViaEventActivateHandlerTests
{
    [Fact]
    public async Task ViaEventActivateHandler_GivenValidCommand_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        var command = ViaEventActivateCommand.Create(id).Payload;

        var eventRepository = new FakeEventRepository();
        eventRepository.AddEvent(ViaEventTestDataFactory.Init(command.Id).WithValidPastDateTimeRange().WithTitle("TEST TITLE").Build());

        var unitOfWork = new FakeUnitOfWork();

        var handler = new ViaEventActivateHandler(eventRepository, unitOfWork);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(eventRepository.Events);

        var viaEvent = eventRepository.Events.First();
        Assert.Equal(command.Id, viaEvent.Id);
        Assert.Equal(ViaEventStatus.Active, viaEvent.Status);
    }
}