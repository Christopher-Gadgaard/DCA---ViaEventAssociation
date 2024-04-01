using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaInvitationId:ViaId
{
    public ViaInvitationId(Guid value) : base(value)
    {
    }
    public static OperationResult<ViaInvitationId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaInvitationId(id);
    }
    public static OperationResult<ViaInvitationId> Create(string id)
    {
        if (Guid.TryParse(id, out var guid))
        {
            return new ViaInvitationId(guid);
        }
        return OperationResult<ViaInvitationId>.Failure( new List<OperationError>( new []{new OperationError(ErrorCode.InvalidInput, "Invalid id")}));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}