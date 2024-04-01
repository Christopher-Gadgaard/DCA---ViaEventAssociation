using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Fakes.Repositories;

public class FakeEventRepository : IViaEventRepository
{
    public List<ViaEvent> Events { get; set; } = new();

    /*public Task<ViaEvent?> GetByIdAsync(ViaId id)
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active).WithVisibility(ViaEventVisibility.Public).Build();
        return Task.FromResult(viaEvent)!;
    }*/

    public Task<ViaEvent?> GetByIdAsync(ViaId id)
    {
        var viaEvent = Events.FirstOrDefault(x => x.Id == id);
        return Task.FromResult(viaEvent);
    }

    public Task AddAsync(ViaEvent entity)
    {
        Events.Add(entity);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(ViaEvent entity)
    {
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ViaId id)
    {
        return Task.CompletedTask;
    }

    public Task<ViaEvent> GetAllAsync()
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).Build();
        return Task.FromResult(viaEvent);
    }
    
    public void AddEvent(ViaEvent viaEvent)
    {
        Events.Add(viaEvent);
    }
}