using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;

public class ViaInvitation
{
    
    internal ViaInvitationId Id { get; private set; }
    internal ViaEventId ViaEventId { get; private set; }
    internal ViaGuestId ViaGuestId { get; private set; }
    internal ViaInvitationStatus Status { get; private set; }
    internal ViaInvitation(ViaInvitationId id, ViaEventId viaEventId, ViaGuestId viaGuestId)
    {
        Id = id;
        ViaEventId = viaEventId;
        ViaGuestId = viaGuestId;
        Status = ViaInvitationStatus.Pending;
    }
    
    public static OperationResult<ViaInvitation> Create(ViaInvitationId invitationId, ViaEventId viaEventId, ViaGuestId viaGuestId)
    {
        return new ViaInvitation( invitationId,viaEventId, viaGuestId);
    }
    public OperationResult<ViaInvitation> Accept()
    {
        Status = ViaInvitationStatus.Accepted;
        return this;
    }
    public OperationResult<ViaInvitation> Reject()
    {
        Status = ViaInvitationStatus.Rejected;
        return this;
    }
}