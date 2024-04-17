using Microsoft.EntityFrameworkCore;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;

namespace EfcDmPersistence;

public abstract class RepositoryEfcBase<TAgg, TId>(DbContext context) : IViaRepository<TAgg, TId>
    where TAgg : AggregateRoot<TId>
    where TId : ViaId
{
    public virtual async Task<TAgg> GetAsync(TId id)
    {
        var root = await context.Set<TAgg>().FindAsync(id);
        return root!;
    }

    public virtual async Task RemoveAsync(TId id)
    {
        var root = await context.Set<TAgg>().FindAsync(id);
        context.Set<TAgg>().Remove(root!);
    }

    public virtual async Task AddAsync(TAgg aggregate)
    {
        await context.Set<TAgg>().AddAsync(aggregate);
    }
}