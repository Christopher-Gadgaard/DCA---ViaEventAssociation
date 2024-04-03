using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Common.Dispatcher;

public class CreateEventMockHandler : ICommandHandler<ViaEventCreateCommand>
{
    public Task<OperationResult> HandleAsync(ViaEventCreateCommand command)
    {
        WasCalled = true;
        return Task.FromResult(OperationResult.Success());
    }

    public bool WasCalled { get; set; }
}