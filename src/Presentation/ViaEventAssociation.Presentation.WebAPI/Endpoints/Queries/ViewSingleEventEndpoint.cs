using Microsoft.AspNetCore.Mvc;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Core.QueryContracts.QueryDispatching;
using ViaEventAssociation.Core.Tools.ObjectMapper;
using ViaEventAssociation.Presentation.WebAPI.Endpoints.Controller;

namespace ViaEventAssociation.Presentation.WebAPI.Endpoints.Queries
{
    public class ViewSingleEventEndpoint(IMapper mapper, IQueryDispatcher dispatcher) : ApiEndpoint.WithRequest<ViewSingleEventRequest>.WithResponse<ViewSingleEventResponse>
    {
        [HttpGet("events/{Id}")]
        public override async Task<ActionResult<ViewSingleEventResponse>> HandleAsync(ViewSingleEventRequest request)
        {
            ViewEvent.Query query = mapper.Map<ViewEvent.Query>(request);
            ViewEvent.Answer answer = await dispatcher.DispatchAsync(query);
            ViewSingleEventResponse response = mapper.Map<ViewSingleEventResponse>(answer);
            return Ok(response);
        }
    }

    public record ViewSingleEventRequest([FromRoute] string Id, [FromQuery] int PageNumber, [FromQuery] int DisplayedRows, [FromQuery] int RowSize);
    public record ViewSingleEventResponse(ViewEvent.Event Event, List<ViewEvent.Guest> Guests, int GuestCount);
}
