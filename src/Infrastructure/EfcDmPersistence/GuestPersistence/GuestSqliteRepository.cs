using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace EfcDmPersistence.GuestPersistence;

public class GuestSqliteRepository(ViaDbContext context) : RepositoryEfcBase<ViaGuest, ViaGuestId>(context)
{
    public OperationResult<ViaGuest> GetByIdAsync(ViaGuestId id)
    {
        var viaGuest = context.Guests.FirstOrDefault(x => x.Id == id);
        return OperationResult<ViaGuest>.Success(viaGuest);
    }
    
    public OperationResult AddGuest(ViaGuest viaGuest)
    {
        context.Guests.Add(viaGuest);
        return OperationResult.Success();
    }
    
 
}