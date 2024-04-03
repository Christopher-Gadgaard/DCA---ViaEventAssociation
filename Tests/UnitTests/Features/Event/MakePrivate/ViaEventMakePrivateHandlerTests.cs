using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.MakePrivate;

public class ViaEventMakePrivateHandlerTests
{
    [Fact]
    public async Task ViaEventMakePrivateHandler_GivenValidCommand_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var command = ViaEventMakePrivateCommand.Create(id).Payload;
  
        var eventRepository = new FakeEventRepository();
        eventRepository.AddEvent(ViaEventTestDataFactory.Init(command.Id).WithVisibility(ViaEventVisibility.Public).Build());
        
        var unitOfWork = new FakeUnitOfWork();
        
        var handler = new ViaEventMakePrivateHandler(eventRepository, unitOfWork);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(eventRepository.Events);
        
        var viaEvent = eventRepository.Events.First();
        Assert.Equal(command.Id, viaEvent.Id);
        Assert.Equal(ViaEventVisibility.Private, viaEvent.Visibility);
    }
}