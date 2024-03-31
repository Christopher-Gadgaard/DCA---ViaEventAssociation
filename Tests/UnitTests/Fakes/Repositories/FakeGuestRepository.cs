using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Fakes.Repositories;

public class FakeGuestRepository:IViaGuestRepository
{
    public Task<ViaGuest?> GetByIdAsync(ViaGuestId id)
    {
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        return Task.FromResult(viaGuest);
    }

    public Task AddAsync(ViaGuest entity)
    {
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ViaGuest entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(ViaGuestId id)
    {
        throw new NotImplementedException();
    }

    public Task<ViaGuest> GetAllAsync()
    {
        throw new NotImplementedException();
    }
}