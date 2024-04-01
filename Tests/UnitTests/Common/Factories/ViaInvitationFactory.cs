using Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationEntity;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace UnitTests.Common.Factories;

public abstract class ViaInvitationFactory
{
    
    public static ViaInvitation CreateValidViaInvitation( ViaEventId viaEventId, ViaGuestId viaGuestId)
    {
        var viaInvitationId = ViaInvitationId.Create().Payload;

        var viaInvitation = new ViaInvitation(viaInvitationId, viaEventId, viaGuestId);
        return viaInvitation;
    }
}