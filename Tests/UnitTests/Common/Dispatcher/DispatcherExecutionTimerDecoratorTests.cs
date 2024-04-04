using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.AppEntry.Dispatcher.Decorator;

namespace UnitTests.Common.Dispatcher;

public class DispatcherExecutionTimerDecoratorTests
{
    [Fact]
    public async Task DispatchAsync_WithDecorator_Success()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<ICommandHandler<ViaEventCreateCommand>, CreateEventMockHandler>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();

        ICommandDispatcher originalDispatcher = new CommandDispatcher(serviceProvider);
        
        ICommandDispatcher decoratedDispatcher = new CommandExecutionTimer(originalDispatcher);
    
        var cmd = ViaEventCreateCommand.Create().Payload;
        
        var result = await decoratedDispatcher.DispatchAsync(cmd);
        
        Assert.True(result.IsSuccess);
        Assert.True(result.ExecutionTimeInMilliseconds > 0);
    }
}