using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaEventId : ViaId
{
    private ViaEventId(Guid value) : base(value)
    {
    }
    public Guid ToGuid()
    {
        return this.Value;
    }
    public static ViaEventId FromGuid(Guid value) => new ViaEventId(value);
    // private ViaEventId(Guid guid)=>Value=guid;
    public static OperationResult<ViaEventId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaEventId(id);
    }

    public static OperationResult<ViaEventId> CreateFromString(string id)
    {
        if (Guid.TryParse(id, out Guid guid))
        {
            return OperationResult<ViaEventId>.Success(new ViaEventId(guid));
        }

        return OperationResult<ViaEventId>.Failure(new List<OperationError>
        {
            new(ErrorCode.InvalidInput, "Invalid id")
        });
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}