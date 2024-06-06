using Microsoft.EntityFrameworkCore;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace EfcDmPersistence.ViaEventPersistence;

public class ViaEventSqliteRepository(DbContext context)
    : RepositoryEfcBase<ViaEvent, ViaEventId>(context), IViaEventRepository
{
    public Task<ViaEvent> GetAsync(ViaId id)
    {
        return context.Set<ViaEvent>().SingleAsync(x => x.Id == id);
    }

    public async Task RemoveAsync(ViaId id)
    {
        var test = await context.Set<ViaEvent>().SingleAsync(x => x.Id == id);
        context.Set<ViaEvent>().Remove(test);
        await context.SaveChangesAsync();
    }

    public override async Task AddAsync(ViaEvent viaEvent)
    {
        await context.Set<ViaEvent>().AddAsync(viaEvent);
        await context.SaveChangesAsync();
    }
}