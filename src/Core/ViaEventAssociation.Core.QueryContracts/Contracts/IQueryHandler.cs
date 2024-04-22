namespace ViaEventAssociation.Core.QueryContracts.Contracts;

public interface IQueryHandler<in TQuery, TAnswer> where TQuery : IQuery<TAnswer>
{
    public Task<TAnswer> HandleAsync(TQuery query);
}