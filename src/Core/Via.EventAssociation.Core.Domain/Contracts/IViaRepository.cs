using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Contracts;

public interface IViaRepository<T, in TId>
//where T : AggregateRoot<TId> //TODO: why cant i do this ?
where TId : ViaId
{
    Task<T> GetAsync(TId id);
    Task RemoveAsync(TId id);
    Task AddAsync(T aggregate);
}