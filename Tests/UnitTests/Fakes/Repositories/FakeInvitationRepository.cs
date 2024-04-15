using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Fakes.Repositories;

public class FakeInvitationRepository:IViaInvitationRepository
{
    public Task<ViaInvitation?> GetByIdAsync(ViaId id)
    {
        var viaInvitation = ViaInvitationFactory.CreateValidViaInvitation(ViaEventId.Create().Payload, ViaGuestId.Create().Payload);
        return Task.FromResult(viaInvitation);
    }

    public Task<ViaInvitation> GetAsync(ViaId id)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(ViaId id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(ViaInvitation entity)
    {
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ViaInvitation entity)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ViaId id)
    {
        return Task.CompletedTask;
    }

    public Task<ViaInvitation> GetAllAsync()
    {
        var viaInvitation = ViaInvitationFactory.CreateValidViaInvitation(ViaEventId.Create().Payload, ViaGuestId.Create().Payload);
        return Task.FromResult(viaInvitation);
    }
}