﻿using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Fakes.Repositories;

public class FakeGuestRepository:IViaGuestRepository
{
    public Task<ViaGuest> GetByIdAsync(ViaGuestId id)
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
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ViaGuestId id)
    {
        return Task.CompletedTask;
    }

    public Task<ViaGuest> GetAllAsync()
    {
        var viaGuest = ViaGuestTestFactory.CreateValidViaGuest();
        return Task.FromResult(viaGuest);
    }
    
}