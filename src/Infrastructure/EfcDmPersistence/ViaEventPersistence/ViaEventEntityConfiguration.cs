using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Via.EventAssociation.Core.Domain.Aggregates.Event;

namespace EfcDmPersistence.ViaEventPersistence;

public class ViaEventEntityConfiguration : IEntityTypeConfiguration<ViaEvent>
{
    public void Configure(EntityTypeBuilder<ViaEvent> builder)
    {
        throw new NotImplementedException();
    }
}