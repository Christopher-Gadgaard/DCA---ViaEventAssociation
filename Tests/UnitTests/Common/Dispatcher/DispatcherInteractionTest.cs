using Microsoft.Extensions.DependencyInjection;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;

namespace UnitTests.Common.Dispatcher;

public class DispatcherInteractionTest
{
    [Fact]
    public async Task DispatchAsync_SingleHandlerRegistered_Success()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddScoped<ICommandHandler<ViaEventCreateCommand>, CreateEventMockHandler>();
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        
        ICommandDispatcher dispatcher = new CommandDispatcher(serviceProvider);
        var cmd = ViaEventCreateCommand.Create().Payload;
        
        var result = await dispatcher.DispatchAsync(cmd);
        
        Assert.True(result.IsSuccess);
        var handler = (CreateEventMockHandler)serviceProvider.GetService<ICommandHandler<ViaEventCreateCommand>>()!;
        Assert.True(handler.WasCalled);
    }
}