using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event.InvitationRequestEntity;

public interface IViaInvitationRequestRepository : IViaRepository<ViaInvitationRequest, ViaInvitationRequestId>
{
    
}