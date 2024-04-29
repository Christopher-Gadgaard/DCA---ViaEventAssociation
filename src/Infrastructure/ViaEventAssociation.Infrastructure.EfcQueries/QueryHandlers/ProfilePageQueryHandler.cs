using EfcDmPersistence;
using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs; 
namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class ProfilePageQueryHandler(VeadatabaseProductionContext context):IQueryHandler<ProfilePage.Query, ProfilePage.Answer>
{
    public async Task<ProfilePage.Answer> HandleAsync(ProfilePage.Query query)
    {
         var guest = await context.Guests.Where(g => g.Id == query.GuestId).Select(g=>new ProfilePage.Guest(g.Id,g.FirstName,g.LastName,g.Email,g.PictureUrl)).FirstOrDefaultAsync();
        
    //guest's approved requests
    var requestEventIds = await context.ViaInvitations
        .Where(i => i.ViaGuestId == query.GuestId && i.Status == "Accepted")
        .Select(i => i.ViaEventId)
        .ToListAsync();
    //events of these requests
    var requestEvents = context.Events
        .Where(e => requestEventIds.Contains(e.Id));
    //     
    //guest's accepted invites
    var inviteEventIds = await context.ViaInvitations
        .Where(i => i.ViaGuestId == query.GuestId && i.Status == "Accepted")
        .Select(i => i.ViaEventId)
        .ToListAsync();
    //events of these invites
    var inviteEvents = context.Events
        .Where(e => inviteEventIds.Contains(e.Id));
    
    var events = requestEvents.Concat(inviteEvents);
    
    //get the total number of invitations for each of these events
    var numberOfInvitations = await context.ViaInvitations
        .Where(i => events.Select(e => e.Id).Contains(i.ViaEventId))
        .GroupBy(i => i.ViaEventId)
        .Select(g => new {EventId = g.Key, Count = g.Count()})
        .ToDictionaryAsync(g => g.EventId, g => g.Count);
    
   
    var guestPendingInvitesCount = context.ViaInvitations
        .Count(i => i.ViaGuestId == query.GuestId && i.Status == "Pending");
    
         return new ProfilePage.Answer(guest!, new List<ProfilePage.Event>(), guestPendingInvitesCount);
     }
}