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
            throw new QueryHandlerNotFoundException(query.GetType().Name, typeof(TAnswer).Name);
        }

        return handler.HandleAsync((dynamic) query);
    }
}
public class QueryHandlerNotFoundException(string queryType, string answerType)
    : Exception($"Query handler for query {queryType} with answer type {answerType} not found.");