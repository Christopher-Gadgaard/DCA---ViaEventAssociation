using System;
using System.Collections.Generic;
using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationRequestEntity;

namespace ViaEventAssociation.Infrastructure.EfcQueries.DTOs;

public partial class Guest
{
    public string Id { get; set; } 

    public string? Email { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? PictureUrl { get; set; }

    public virtual ICollection<ViaInvitation> Invites { get; set; } = new List<ViaInvitation>();

    public virtual ICollection<ViaInvitationRequest> Requests { get; set; } = new List<ViaInvitationRequest>();
}
