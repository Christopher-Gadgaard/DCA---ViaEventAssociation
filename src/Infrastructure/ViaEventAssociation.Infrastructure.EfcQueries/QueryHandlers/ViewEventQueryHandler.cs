using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class ViewEventQueryHandler
    (VeadatabaseProductionContext context) : IQueryHandler<ViewEvent.Query, ViewEvent.Answer>
{
    public async Task<ViewEvent.Answer> HandleAsync(ViewEvent.Query query)
    {
        var singleEvent = await context.Events.Where(e => e.Id == query.EventId).Select(e =>
                new ViewEvent.Event(e.EventTitle, e.EventDescription, e.StartDate, e.EndDate, e.Visibility,
                    e.MaxGuests))
            .FirstOrDefaultAsync();
        
        var inviteGuestIds = await context.ViaInvitations
            .Where(i => i.ViaEventId == query.EventId && i.Status == "Accepted")
            .Select(i => i.ViaGuestId)
            .ToListAsync();
       
        var invitedGuests = context.Guests
            .Where(g => inviteGuestIds.Contains(g.Id));
        
    return new ViewEvent.Answer(singleEvent!, new List<ViewEvent.Guest>(), invitedGuests.Count());

    }
}