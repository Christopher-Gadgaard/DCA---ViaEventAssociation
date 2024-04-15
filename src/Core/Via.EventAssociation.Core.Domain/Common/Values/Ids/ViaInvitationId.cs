using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaInvitationId:ViaId
{
    private ViaInvitationId(Guid value) : base(value)
    {
        
    }
    
    public static ViaInvitationId FromGuid(Guid value) => new(value);

    public Guid ToGuid()
    {
        return this.Value;
    }
    public static OperationResult<ViaInvitationId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaInvitationId(id);
    }
    public static OperationResult<ViaInvitationId> Create(string id)
    {
        return Guid.TryParse(id, out var guid) ? new ViaInvitationId(guid) : OperationResult<ViaInvitationId>.Failure( new List<OperationError>( new []{new OperationError(ErrorCode.InvalidInput, "Invalid id")}));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}