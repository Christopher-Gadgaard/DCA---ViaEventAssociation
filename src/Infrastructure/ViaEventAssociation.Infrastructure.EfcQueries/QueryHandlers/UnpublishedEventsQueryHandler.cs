using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class UnpublishedEventsQueryHandler(VeadatabaseProductionContext context): IQueryHandler<UnpublishedEvents.Query, UnpublishedEvents.Answer>
{
    public async Task<UnpublishedEvents.Answer> HandleAsync(UnpublishedEvents.Query query)
    {
        var draftEvents = await context.Events
            .Where(e => e.Status == "Draft")
            .Select(e => new UnpublishedEvents.Event(e.Id, e.EventTitle, e.Status))
            .ToListAsync();
        
        var readyEvents = await context.Events
            .Where(e => e.Status == "Ready")
            .Select(e => new UnpublishedEvents.Event(e.Id, e.EventTitle, e.Status))
            .ToListAsync();
        
        var cancelledEvents = await context.Events
            .Where(e => e.Status == "Cancelled")
            .Select(e => new UnpublishedEvents.Event(e.Id, e.EventTitle, e.Status))
            .ToListAsync();
        
        return new UnpublishedEvents.Answer(draftEvents, readyEvents, cancelledEvents);
    }
}