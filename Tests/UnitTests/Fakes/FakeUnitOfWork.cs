using Via.EventAssociation.Core.Domain.Common.UnitOfWork;

namespace UnitTests.Fakes;

public class FakeUnitOfWork : IUnitOfWork
{
    public Task SaveChangesAsync()
    {
        return Task.CompletedTask;
    }
}