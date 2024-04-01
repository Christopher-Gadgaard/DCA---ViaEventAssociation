using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.CreateEvent;

public class ViaEventCreateHandlerTests
{
    [Fact]
    public async Task ViaEventCreationHandler_GivenValidCommand_Success_WithSetIdAndDefaultValues()
    {
        // Arrange
        var command = ViaEventCreateCommand.Create().Payload;
        var eventRepository = new FakeEventRepository();
        var unitOfWork = new FakeUnitOfWork();
        var handler = new ViaEventCreateHandler(eventRepository, unitOfWork);

        // Act
        var result = await handler.Handle(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(eventRepository.Events);

        var viaEvent = eventRepository.Events.First();
        Assert.Equal(command.Id, viaEvent.Id);
        Assert.Equal(ViaEventTitle.Default(), viaEvent.Title);
        Assert.Equal(ViaEventDescription.Default(), viaEvent.Description);
        Assert.Equal(ViaMaxGuests.Default(), viaEvent.MaxGuests);
        Assert.Equal(ViaEventStatus.Draft, viaEvent.Status);
        Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
    }
}