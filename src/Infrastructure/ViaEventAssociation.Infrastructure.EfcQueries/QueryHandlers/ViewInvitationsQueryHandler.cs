using Microsoft.EntityFrameworkCore;
using ViaEventAssociation.Core.QueryContracts.Contracts;
using ViaEventAssociation.Core.QueryContracts.Queries;
using ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

namespace ViaEventAssociation.Infrastructure.EfcQueries.QueryHandlers;

public class ViewInvitationsQueryHandler(VeadatabaseProductionContext context): IQueryHandler<ViewInvitations.Query, ViewInvitations.Answer>
{
    public async Task<ViewInvitations.Answer> HandleAsync(ViewInvitations.Query query)
    {
        var invitations = await context.ViaInvitations
            .Where(i => i.ViaGuestId == query.UserId)
            .Select(i => new ViewInvitations.ViaInvitation( 
                i.Id,
                i.ViaEventId,
                i.ViaEvent.EventTitle,
                i.ViaEvent.StartDate,
                i.ViaEvent.EndDate,
                i.ViaEvent.Visibility,
                i.Status))
            .ToListAsync();
        
        
        
        return new ViewInvitations.Answer(invitations, invitations.Count);
    }
}