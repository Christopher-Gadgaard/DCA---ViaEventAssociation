using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class VeadatabaseProductionContext : DbContext
{
    public VeadatabaseProductionContext()
    {
    }

    public VeadatabaseProductionContext(DbContextOptions<VeadatabaseProductionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<ViaGuestId> ViaGuestIds { get; set; }

    public virtual DbSet<ViaInvitation> ViaInvitations { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source =C:\\DCA1\\DCA-ViaEventAssociation\\src\\Infrastructure\\EfcDmPersistence\\VEADatabaseProduction.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ViaGuestId>(entity =>
        {
            entity.ToTable("ViaGuestId");

            entity.HasIndex(e => e.ViaEventId, "IX_ViaGuestId_ViaEventId");

            entity.HasOne(d => d.ViaEvent).WithMany(p => p.ViaGuestIds).HasForeignKey(d => d.ViaEventId);
        });

        modelBuilder.Entity<ViaInvitation>(entity =>
        {
            entity.ToTable("ViaInvitation");

            entity.HasIndex(e => e.ViaEventId, "IX_ViaInvitation_ViaEventId");

            entity.HasOne(d => d.ViaEvent).WithMany(p => p.ViaInvitations).HasForeignKey(d => d.ViaEventId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
