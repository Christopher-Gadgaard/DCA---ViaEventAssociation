using Microsoft.AspNetCore.Mvc;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using ViaEventAssociation.Core.AppEntry;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.AppEntry.Dispatcher;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Controller;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Event
{

    public class ActivateEventEndpoint(ICommandDispatcher dispatcher, IUnitOfWork unitOfWork) : ApiEndpoint.WithRequest<ActivateEventRequest>.WithoutResponse
    {
        [HttpPost("events/{EventId}/activate")]
        public override async Task<ActionResult> HandleAsync([FromRoute] ActivateEventRequest activateEventRequest)
        {
            var commandResult = ViaEventActivateCommand.Create(activateEventRequest.EventId);
            if (commandResult.IsFailure)
            {
                return BadRequest(commandResult.OperationErrors);
            }

            var saveDispatcher = new UowSaveDispatcher(dispatcher, unitOfWork);
            var dispatchResult = await saveDispatcher.DispatchAsync(commandResult);
            return !dispatchResult.IsFailure ? Ok() : BadRequest(dispatchResult);
        }
    }

    public record ActivateEventRequest([FromRoute] string EventId);
}