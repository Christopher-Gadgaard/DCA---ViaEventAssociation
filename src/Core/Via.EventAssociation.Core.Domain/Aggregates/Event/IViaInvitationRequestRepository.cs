﻿using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;

namespace Via.EventAssociation.Core.Domain.Contracts;

public interface IViaInvitationRequestRepository : IViaRepository<ViaInvitationRequest, ViaInvitationRequestId>
{
    
}