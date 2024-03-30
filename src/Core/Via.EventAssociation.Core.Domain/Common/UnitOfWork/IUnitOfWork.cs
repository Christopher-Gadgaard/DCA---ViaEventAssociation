namespace Via.EventAssociation.Core.Domain.Common.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}