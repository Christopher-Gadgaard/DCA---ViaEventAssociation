using System;
using System.Collections.Generic;

namespace ViaEventAssociation.Infrastructure.EfcQueries;

public partial class ViaInvitation
{
    public string Id { get; set; } = null!;

    public string ViaEventId { get; set; } = null!;

    public string ViaGuestId { get; set; } = null!;

    public string Status { get; set; } = null!;

    public virtual Event ViaEvent { get; set; } = null!;
}
