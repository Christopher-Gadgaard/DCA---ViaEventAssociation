using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace EfcDmPersistence.ViaEventPersistence;

public class ViaEventEntityConfiguration : IEntityTypeConfiguration<ViaEvent>
{
    public void Configure(EntityTypeBuilder<ViaEvent> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder
            .Property(m => m.Id)
            .HasConversion(mId => mId.Value, dbValue => ViaEventId.FromGuid(dbValue));

        builder.OwnsOne(m => m.Title, a => { a.Property(t => t.Value).HasColumnName("EventTitle"); });

        builder.OwnsOne(m => m.Description, a => { a.Property(t => t.Value).HasColumnName("EventDescription"); });

        builder.OwnsOne(m => m.DateTimeRange, dateTimeRange =>
        {
            dateTimeRange.Property(d => d.StartValue).HasColumnName("StartDate");
            dateTimeRange.Property(d => d.EndValue).HasColumnName("EndDate");
        });

        builder.OwnsOne(m => m.MaxGuests, a => { a.Property(t => t.Value).HasColumnName("MaxGuests"); });

        builder
            .Property(e => e.Status)
            .HasConversion(
                e => e.ToString(),
                e => (ViaEventStatus) Enum.Parse(typeof(ViaEventStatus), e)
            );

        builder
            .Property(e => e.Visibility)
            .HasConversion(
                e => e.ToString(),
                e => (ViaEventVisibility) Enum.Parse(typeof(ViaEventVisibility), e)
            );

        builder.OwnsMany(m => m.Guests, a =>
        {
            a.WithOwner().HasForeignKey("ViaEventId");
            a.Property<int>("Id").ValueGeneratedOnAdd();
            a.HasKey("Id");
        });


        builder.OwnsMany<ViaInvitation>("Invitations", invitations =>
        {
            invitations.HasKey(inv => inv.Id);
            invitations.Property(inv => inv.Id)
                .HasConversion(
                    mId => mId.Value,
                    dbValue => ViaInvitationId.FromGuid(dbValue)
                );

            invitations.WithOwner().HasForeignKey(inv => inv.ViaEventId);
            invitations
                .Property(inv => inv.ViaEventId)
                .HasConversion(
                    mId => mId.Value,
                    dbValue => ViaEventId.FromGuid(dbValue)
                );

           // invitations.WithOwner().HasForeignKey(inv => inv.ViaGuestId);
            invitations.Property(inv => inv.ViaGuestId)
                .HasConversion(mId => mId.Value, dbValue => ViaGuestId.FromGuid(dbValue));

            invitations.Property<ViaInvitationStatus>("Status")
                .HasConversion(
                    status => status.ToString(),
                    status => (ViaInvitationStatus) Enum.Parse(typeof(ViaInvitationStatus), status)
                )
                .HasColumnName("Status");
        });
    }
}