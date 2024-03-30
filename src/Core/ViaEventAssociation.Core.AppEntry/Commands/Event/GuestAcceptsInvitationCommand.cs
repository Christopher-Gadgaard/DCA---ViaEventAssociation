﻿using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class GuestAcceptsInvitationCommand
{
    public ViaEventId EventId { get;}
    public ViaGuestId GuestId { get;}
    public ViaInvitationId InvitationId { get;}
    
    public GuestAcceptsInvitationCommand(ViaEventId eventId, ViaGuestId guestId, ViaInvitationId invitationId)
    {
        EventId = eventId;
        GuestId = guestId;
        InvitationId = invitationId;
    }
    
    public static OperationResult<GuestAcceptsInvitationCommand> Create(string eventId, string guestId, string invitationId)
    {
        OperationResult<ViaEventId> eventResult = ViaEventId.Create();
        OperationResult<ViaGuestId> guestResult = ViaGuestId.Create();
        OperationResult<ViaInvitationId> invitationResult = ViaInvitationId.Create();
        OperationResult<GuestAcceptsInvitationCommand> combinedResult = OperationResult<GuestAcceptsInvitationCommand>.Combine(eventResult.OperationErrors, guestResult.OperationErrors, invitationResult.OperationErrors);

        return combinedResult;
    }
}