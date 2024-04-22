using Via.EventAssociation.Core.Domain.Aggregates.Guests.Values;
using Via.EventAssociation.Core.Domain.Common.Bases;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests;

public class ViaGuest:AggregateRoot<ViaGuestId>
{
    
    private ViaGuestName _viaGuestName;
    private ViaEmail _viaEmail;
    private ViaProfilePicUrl _viaProfilePicUrl;
    internal new ViaGuestId Id => base.Id;
    internal ViaGuestName ViaGuestName => _viaGuestName;
    internal ViaEmail ViaEmail => _viaEmail;
    
    private ViaGuest(){}
 
    internal ViaGuest( ViaGuestId viaGuestId, ViaGuestName viaGuestName, ViaEmail viaEmail, ViaProfilePicUrl profilePicUrl):base(viaGuestId)
    {
        _viaGuestName = viaGuestName;
        _viaEmail = viaEmail;
        _viaProfilePicUrl = profilePicUrl;
    }
    
    public static OperationResult<ViaGuest> Create(ViaGuestId viaGuestId, OperationResult<ViaGuestName> viaGuestNameResult, OperationResult<ViaEmail> viaEmailResult, OperationResult<ViaProfilePicUrl> viaProfilePicUrlResult)
    {
      
        if (!viaGuestNameResult.IsSuccess)
        {
            return OperationResult<ViaGuest>.Failure(viaGuestNameResult.OperationErrors);
        }

    
        if (!viaEmailResult.IsSuccess)
        {
            return OperationResult<ViaGuest>.Failure(viaEmailResult.OperationErrors);
        }

        if (!viaProfilePicUrlResult.IsSuccess)
        {
            return OperationResult<ViaGuest>.Failure(viaProfilePicUrlResult.OperationErrors);
        }

        var guest = new ViaGuest(viaGuestId, viaGuestNameResult.Payload, viaEmailResult.Payload, viaProfilePicUrlResult.Payload);
        return OperationResult<ViaGuest>.Success(guest);
    }
    
}