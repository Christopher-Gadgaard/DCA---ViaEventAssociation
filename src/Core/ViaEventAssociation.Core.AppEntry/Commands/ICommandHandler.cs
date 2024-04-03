using System.Reflection.Metadata;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands;

public interface ICommandHandler<in TCommand>
{
     Task<OperationResult> HandleAsync(TCommand command);
}