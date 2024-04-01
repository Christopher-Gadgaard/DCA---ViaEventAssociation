using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.SetMaxGuests;

public class ViaEventSetMaxGuestsHandlerTests
{
    [Fact]
    public async Task ViaEventSetMaxGuestsHandler_GivenValidCommand_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const int maxGuests = 10;

        var command = ViaEventSetMaxGuestsCommand.Create(id, maxGuests).Payload;

        var eventRepository = new FakeEventRepository();
        eventRepository.AddEvent(ViaEventTestDataFactory.Init(command.Id).WithMaxGuests(maxGuests).Build());

        var unitOfWork = new FakeUnitOfWork();

        var handler = new ViaEventSetMaxGuestsHandler(eventRepository, unitOfWork);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(eventRepository.Events);

        var viaEvent = eventRepository.Events.First();
        Assert.Equal(command.Id, viaEvent.Id);
        Assert.Equal(command.MaxGuests, viaEvent.MaxGuests);
    }
}