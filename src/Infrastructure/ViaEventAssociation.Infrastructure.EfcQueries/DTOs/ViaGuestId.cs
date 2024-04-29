using System;
using System.Collections.Generic;

namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class ViaGuestId
{
    public int Id { get; set; }

    public string ViaEventId { get; set; } = null!;

    public virtual Event ViaEvent { get; set; } = null!;
}
