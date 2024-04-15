using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Values;
using Via.EventAssociation.Core.Domain.Common.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace EfcDmPersistence.ViaEventPersistence;

public class ViaEventEntityConfiguration : IEntityTypeConfiguration<ViaEvent>
{
    public void Configure(EntityTypeBuilder<ViaEvent> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder
            .Property(m => m.Id).HasConversion(mId => mId.Value, dbValue => ViaEventId.FromGuid(dbValue));

        builder.ComplexProperty<ViaEventTitle>(
            "Title",
            propBuilder => { propBuilder.Property(email => email.Value).HasColumnName("Event Title"); });

        builder.ComplexProperty<ViaEventDescription>(
            "Description",
            propBuilder => { propBuilder.Property(email => email.Value).HasColumnName("Event Description"); });

        builder.OwnsOne<ViaDateTimeRange>("DateTimeRange", propBuilder =>
        {
            propBuilder.Property(start => start.StartValue)
                .HasColumnName("Start Date");

            propBuilder.Property(end => end.EndValue)
                .HasColumnName("End Date");
        });

        builder.ComplexProperty<ViaMaxGuests>(
            "MaxGuests",
            propBuilder => { propBuilder.Property(email => email.Value).HasColumnName("Max Guest"); });


        builder.Property<ViaEventStatus>("Status").HasConversion(status => status.ToString(),
            value => (ViaEventStatus) Enum.Parse(typeof(ViaEventStatus), value));

        builder.Property<ViaEventVisibility>("Visibility").HasConversion(visibility => visibility.ToString(),
            value => (ViaEventVisibility) Enum.Parse(typeof(ViaEventVisibility), value));


        builder.OwnsMany<ViaGuestId>("Guests", valueBuilder =>
        {
            valueBuilder.Property<int>("Id").ValueGeneratedOnAdd();
            valueBuilder.HasKey("Id");
            valueBuilder.Property(x => x.Value);
        });

        /*builder.OwnsMany<ViaInvitation>("Invitations", valueBuilder =>
        {
            valueBuilder.Property<int>("Id").ValueGeneratedOnAdd();
            valueBuilder.HasKey("Id");
            valueBuilder.Property(x => x.Value);
        });*/
    }
}