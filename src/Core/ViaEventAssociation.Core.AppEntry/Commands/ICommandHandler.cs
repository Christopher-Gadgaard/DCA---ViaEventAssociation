using System.Reflection.Metadata;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands;

public interface ICommandHandler<TCommand>
{
     Task<OperationResult> Handle(TCommand command);
}