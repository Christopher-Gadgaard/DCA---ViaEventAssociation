using System;
using System.Collections.Generic;

namespace ViaEventAssociation.Infrastructure.EfcQueries;

public partial class Event
{
    public string Id { get; set; } = null!;

    public string EventTitle { get; set; } = null!;

    public string EventDescription { get; set; } = null!;

    public string? StartDate { get; set; }

    public string? EndDate { get; set; }

    public int MaxGuests { get; set; }

    public string Status { get; set; } = null!;

    public string Visibility { get; set; } = null!;

    public virtual ICollection<ViaGuestId> ViaGuestIds { get; set; } = new List<ViaGuestId>();

    public virtual ICollection<ViaInvitation> ViaInvitations { get; set; } = new List<ViaInvitation>();
}
