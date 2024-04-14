using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Aggregates.Guests.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace EfcDmPersistence.GuestPersistence;

public class ViaGuestEntityConfiguration : IEntityTypeConfiguration<ViaGuest>
{

    public void Configure(EntityTypeBuilder<ViaGuest> builder)
    {
        builder.HasKey(entity => entity.Id);

        builder
            .Property(m => m.Id).HasConversion(mId => mId.Value, dbValue => ViaGuestId.FromGuid(dbValue));

        builder.ComplexProperty<ViaGuestName>(
            "ViaGuestName", propBuilder =>
            {
                propBuilder.ComplexProperty(x => x.FirstName).Property(x => x.Value)
                    .HasColumnName("FirstName");

                propBuilder.ComplexProperty(x => x.LastName).Property(x => x.Value)
                    .HasColumnName("LastName");
            });
                

        builder.ComplexProperty<ViaEmail>(
            "ViaEmail", propBuilder => { propBuilder.Property(email => email.Value).HasColumnName("Email"); 
        });
        
        

    }
}