using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher;

internal class CommandDispatcher(IServiceProvider serviceProvider): ICommandDispatcher
{
    public Task<OperationResult> DispatchAsync<TCommand>(TCommand command)
    {
        var serviceType = typeof(ICommandHandler<TCommand>);
        var service = serviceProvider.GetService(serviceType);
        if (service == null)
        {
            throw new InvalidOperationException($"Handler for {serviceType.Name} not found");
        }
        var handler = (ICommandHandler<TCommand>)service;
        return handler.HandleAsync(command);
    }
}