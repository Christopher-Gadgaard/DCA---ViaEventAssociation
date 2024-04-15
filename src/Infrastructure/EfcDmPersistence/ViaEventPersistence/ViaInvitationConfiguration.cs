using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace EfcDmPersistence.ViaEventPersistence;

public class ViaInvitationConfiguration : IEntityTypeConfiguration<ViaInvitation>
{
    public void Configure(EntityTypeBuilder<ViaInvitation> builder)
    {
        builder.HasKey(inv => inv.Id);

        builder.Property(inv => inv.ViaEventId)
            .HasColumnName("EventId")
            .HasConversion(
                id => id.ToGuid(),  // Assuming you have a ToGuid method in ViaEventId
                guid => ViaEventId.FromGuid(guid)  // Static method to create ViaEventId from Guid
            );

        builder.HasOne<ViaEvent>()  // Configures the relationship to ViaEvent
            .WithMany()  // Assuming ViaEvent has a collection of ViaInvitations
            .HasForeignKey(inv => inv.ViaEventId);

        // Add other property configurations as needed
    }
}