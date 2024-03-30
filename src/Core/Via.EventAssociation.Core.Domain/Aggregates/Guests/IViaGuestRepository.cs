using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;

public interface IViaGuestRepository: IViaRepository<ViaGuest, ViaGuestId>
{
    
}