using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Contracts;

public interface IViaRepository<T, in TViaId>
{
    Task<T?> GetByIdAsync(TViaId id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(TViaId id);
    Task<T> GetAllAsync();
}