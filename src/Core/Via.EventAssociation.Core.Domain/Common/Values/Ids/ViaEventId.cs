using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Common.Values.Ids;

public class ViaEventId : ViaId
{
    private ViaEventId(Guid value) : base(value)
    {
        
    }

    public static OperationResult<ViaEventId> Create()
    {
        var id = Guid.NewGuid();
        return new ViaEventId(id);
    }

    public static OperationResult<ViaEventId> Create(string id)
    {
        if (Guid.TryParse(id, out Guid guid))
        {
            return OperationResult<ViaEventId>.Success(new ViaEventId(guid));
        }
        else
        {
            return OperationResult<ViaEventId>.Failure(new List<OperationError>
            {
                new OperationError(ErrorCode.InvalidInput, "Invalid id")
            });
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}