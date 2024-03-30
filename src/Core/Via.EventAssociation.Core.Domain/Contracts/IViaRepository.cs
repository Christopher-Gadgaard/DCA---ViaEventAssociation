using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Contracts;

public interface IViaRepository<T, ViaId>
{
 
   Task<T?> GetByIdAsync(ViaId id);
    Task AddAsync(T entity);    
   Task UpdateAsync(T entity);
   Task DeleteAsync(ViaId id);
    Task<T> GetAllAsync();
    

}