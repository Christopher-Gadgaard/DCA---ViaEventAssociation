using EfcDmPersistence;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace Integration_Tests.ViaDbContextConfigurationTests;

public class ViaEventConfigurationTests
{
    [Fact]
    public async Task GuidAsPk()
    {
        await using var ctx = GlobalUsings.SetupContext();

        var eventId = ViaEventId.Create();
        var viaEvent = ViaEvent.Create(eventId.Payload);
        await GlobalUsings.SaveAndClearAsync(viaEvent, ctx);
        var retrieved = ctx.Events.SingleOrDefault(x => x.Id == viaEvent.Payload.Id);
        Assert.NotNull(retrieved);
    }
}