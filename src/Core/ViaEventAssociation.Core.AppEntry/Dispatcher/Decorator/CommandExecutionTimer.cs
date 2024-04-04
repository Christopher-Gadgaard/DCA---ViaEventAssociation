using System.Diagnostics;
using System.Windows.Input;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Dispatcher.Decorator;

public class CommandExecutionTimer(ICommandDispatcher next):ICommandDispatcher
{
    private float ElapsedMilliseconds { get; set; }
    public async Task<OperationResult> DispatchAsync<TCommand>(TCommand command)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var result = await next.DispatchAsync(command);
        ElapsedMilliseconds = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Command {command?.GetType().Name} executed in {ElapsedMilliseconds} ms");
        result.AppendExecutionTime(ElapsedMilliseconds);
        return result;
    }
}