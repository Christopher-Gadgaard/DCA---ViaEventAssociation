using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.QueryDispatching;

public interface IQueryDispatcher
{
    Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query);
}