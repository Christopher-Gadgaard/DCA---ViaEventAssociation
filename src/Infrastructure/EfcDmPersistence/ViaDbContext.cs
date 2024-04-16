using EfcDmPersistence.GuestPersistence;
using EfcDmPersistence.ViaEventPersistence;
using Microsoft.EntityFrameworkCore;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;

namespace EfcDmPersistence;

public class ViaDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ViaEvent> Events => Set<ViaEvent>();
    public DbSet<ViaGuest> Guests => Set<ViaGuest>();
    // public DbSet<ViaInvitation> Invitations { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ViaDbContext).Assembly);
        // modelBuilder.ApplyConfiguration(new ViaEventEntityConfiguration());
        // modelBuilder.ApplyConfiguration(new ViaGuestEntityConfiguration());
        // modelBuilder.ApplyConfiguration(new ViaInvitationConfiguration());
    }
}