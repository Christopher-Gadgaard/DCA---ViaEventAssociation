using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class BrowseEventsQueryHandler
    (VeadatabaseProductionContext context) : IQueryHandler<BrowseEvents.Query, BrowseEvents.Answer>
{
    public async Task<BrowseEvents.Answer> HandleAsync(BrowseEvents.Query query)
    {
        var time = DateTime.Now;
        var allUpcomingEvents = context.Events
            .Where(e => 
                e.StartDate.CompareTo(time) > 0 
                && e.EventTitle.Contains(query.SearchedText)
            );

        var upcomingEvents = allUpcomingEvents
            .OrderByDescending(e => e.StartDate);
        
        //get the total number of invitations for each of these events
        var numberOfInvitations = await context.ViaInvitations
            .Where(i => upcomingEvents.Select(e => e.Id).Contains(i.ViaEventId))
            .GroupBy(i => i.ViaEventId)
            .Select(g => new {EventId = g.Key, Count = g.Count()})
            .ToDictionaryAsync(g => g.EventId, g => g.Count);
        
       
        
        //combine the events with the number of invitations and requests
        var upcomingEventsWithParticipants = await upcomingEvents.Select(e => new BrowseEvents.Event(
                e.Id,
                e.EventTitle,
                e.EventDescription,
                e.MaxGuests,
                numberOfInvitations.GetValueOrDefault(e.Id, 0),
                e.StartDate
            )).ToListAsync();
        var maxPageNum = allUpcomingEvents.Count() / query.PageSize;
        if (maxPageNum % query.PageSize > 0)
        {
            maxPageNum++;
        }

  
        return new BrowseEvents.Answer(upcomingEventsWithParticipants, maxPageNum);
    }
}
