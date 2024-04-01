using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;

namespace Via.EventAssociation.Core.Domain.Aggregates.Event;

public interface IViaEventRepository: IViaRepository<ViaEvent, ViaId>
{
    
}