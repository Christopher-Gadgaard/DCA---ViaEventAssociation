﻿using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Event;

public class InviteGuestCommand
{
    public ViaEventId EventId { get;}
    public ViaGuestId GuestId { get;}
    
    public InviteGuestCommand(ViaEventId eventId, ViaGuestId guestId)
    {
        EventId = eventId;
        GuestId = guestId;
    }

    public static OperationResult<InviteGuestCommand> Create(string eventId, string guestId)
    {
        OperationResult<ViaEventId> eventResult = ViaEventId.CreateFromString(eventId);
        OperationResult<ViaGuestId> guestResult = ViaGuestId.Create(guestId);
        OperationResult<InviteGuestCommand> combinedResult = OperationResult<InviteGuestCommand>.Combine(eventResult.OperationErrors, guestResult.OperationErrors);

        if(combinedResult.IsSuccess)
        {
            return OperationResult<InviteGuestCommand>.Success(new InviteGuestCommand(eventResult.Payload, guestResult.Payload));
        }

        return combinedResult;
    }
}