using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry
{
    public class UowSaveDispatcher(ICommandDispatcher next, IUnitOfWork unitOfWork) : ICommandDispatcher
    {
        public async Task<OperationResult> DispatchAsync<TCommand>(TCommand command)
        {
            var dispatchResult = await next.DispatchAsync(command);
            if (dispatchResult.IsFailure)
            {
                return dispatchResult;
            }

            await unitOfWork.SaveChangesAsync();
            return OperationResult.Success();
        }
    }
}
