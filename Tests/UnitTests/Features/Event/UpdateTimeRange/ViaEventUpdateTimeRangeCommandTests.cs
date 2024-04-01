using UnitTests.Common.Factories;
using UnitTests.Common.Utilities;
using ViaEventAssociation.Core.AppEntry.Commands.Event;

namespace UnitTests.Features.Event.UpdateTimeRange;

public class ViaEventUpdateTimeRangeCommandTests
{
    [Fact]
    public void ViaEventUpdateTimeRangeCommand_GivenValidIdAndTimeRange_Success()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        var timeRange = ViaDateTimeRangeTestDataFactory.CreateValidDateRange();
        var timeProvider = new FakeTimeProvider(timeRange.start.AddDays(-1));
        
        // Act
        var result = ViaEventUpdateTimeRangeCommand.Create(id, timeRange.start, timeRange.end, timeProvider);
        var command = result.Payload;
        
        // Assert
        Assert.True(result.IsSuccess);
        
        Assert.NotNull(command.Id);
        Assert.NotNull(command.Id.ToString());
        Assert.NotEmpty(command.Id.ToString()!);
        Assert.Equal(id, command.Id.Value.ToString());
        
        Assert.NotNull(command.DateTimeRange);
        Assert.Equal(timeRange.start, command.DateTimeRange.StartValue);
        Assert.Equal(timeRange.end, command.DateTimeRange.EndValue);
    }
}