using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher;

public interface ICommandDispatcher
{
    Task<OperationResult> DispatchAsync<TCommand>(TCommand command);
}