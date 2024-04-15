using Via.EventAssociation.Core.Domain.Common.UnitOfWork;

namespace EfcDmPersistence.UnitOfWork;

public class SqliteUnitOfWork(ViaDbContext context):IUnitOfWork
{
    public Task SaveChangesAsync() => context.SaveChangesAsync();
}