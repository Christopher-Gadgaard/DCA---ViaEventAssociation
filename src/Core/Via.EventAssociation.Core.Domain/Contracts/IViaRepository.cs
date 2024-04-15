using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Contracts;

public interface IViaRepository<T, in TId>
{
    Task<T> GetAsync(TId id);
    Task RemoveAsync(TId id);
    Task AddAsync(T aggregate);
}