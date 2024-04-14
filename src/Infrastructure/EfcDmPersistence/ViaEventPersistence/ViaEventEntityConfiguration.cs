using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace EfcDmPersistence.ViaEventPersistence;

public class ViaEventEntityConfiguration : IEntityTypeConfiguration<ViaEvent>
{
    public void Configure(EntityTypeBuilder<ViaEvent> builder)
    {
        builder.HasKey(entity=>entity.Id);
        
        builder
            .Property(m=>m.Id).HasConversion(mId=>mId.Value, dbValue=>ViaEventId.FromGuid(dbValue));
        
    }
}