using UnitTests.Common.Factories;
using UnitTests.Common.Utilities;
using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Event.UpdateTimeRange;

public class ViaEventUpdateTimeRangeHandlerTests
{
    [Fact]
    public async Task ViaEventUpdateTimeRangeHandler_GivenValidCommand_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var timeRange = ViaDateTimeRangeTestDataFactory.CreateValidDateRange();
        var timeProvider = new FakeTimeProvider(timeRange.start.AddDays(-1));

        var command = ViaEventUpdateTimeRangeCommand.Create(id, timeRange.start, timeRange.end, timeProvider).Payload;

        var eventRepository = new FakeEventRepository();
        eventRepository.AddEvent(ViaEventTestDataFactory.Init(command.Id).WithValidPastDateTimeRange().Build());

        var unitOfWork = new FakeUnitOfWork();

        var handler = new ViaEventUpdateTimeRangeHandler(eventRepository, unitOfWork);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(eventRepository.Events);

        var viaEvent = eventRepository.Events.First();
        Assert.Equal(command.Id, viaEvent.Id);
        Assert.Equal(command.DateTimeRange, viaEvent.DateTimeRange);
    }
}