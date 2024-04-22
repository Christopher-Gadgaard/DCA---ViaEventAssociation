using ViaEventAssociation.Core.QueryContracts.Contracts;

namespace ViaEventAssociation.Core.QueryContracts.QueryDispatching;

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query)
    {
        var queryInterfaceWithTypes = typeof(IQueryHandler<,>)
            .MakeGenericType(query.GetType(), typeof(TAnswer));

        dynamic handler = serviceProvider.GetService(queryInterfaceWithTypes)!;

        if (handler == null)
        {
            throw new NotImplementedException();
        }

        return handler.HandleAsync((dynamic) query);
    }
}