using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher.Decorator;

public class CommandSaveChanges(ICommandDispatcher next):ICommandDispatcher
{
    public async Task<OperationResult> DispatchAsync<TCommand>(TCommand command)
    {
        var result = await next.DispatchAsync(command);
        if (result.IsFailure)
        {
            return result;
        }

        return result;
    }
}