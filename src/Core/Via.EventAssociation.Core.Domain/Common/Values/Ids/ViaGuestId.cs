using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaGuestId : ViaId
{
    private ViaGuestId(Guid value) : base(value)
    {
    }
    public Guid ToGuid()
    {
        return Value;
    }

    private ViaGuestId()
    {
        
    }
    public static ViaGuestId FromGuid(Guid value) => new ViaGuestId(value);

    public static OperationResult<ViaGuestId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaGuestId(id);
    }

    public static OperationResult<ViaGuestId> Create(string id)
    {
        if (Guid.TryParse(id, out Guid guid))
        {
            return OperationResult<ViaGuestId>.Success(new ViaGuestId(guid));
        }

        return OperationResult<ViaGuestId>.Failure(new List<OperationError>
        {
            new(ErrorCode.InvalidInput, "Invalid id")
        });
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}