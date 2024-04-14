using Via.EventAssociation.Core.Domain.Common.Bases;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class ViaGuestName : ValueObject
{
    public ViaName FirstName { get; private set; }
    public ViaName LastName { get; private set;}
    
    private ViaGuestName(){}

    private ViaGuestName(ViaName guestFirstName, ViaName guestLastName)
    {
        FirstName = guestFirstName;
        LastName = guestLastName;
    }

    public static OperationResult<ViaGuestName> Create(string guestFirstName, string guestLastName)
    {
        var firstNameResult = ViaName.Create(guestFirstName);
        var lastNameResult = ViaName.Create(guestLastName);

        if (!firstNameResult.IsSuccess)
        {
            return OperationResult<ViaGuestName>.Failure(firstNameResult.OperationErrors);
        }

        if (!lastNameResult.IsSuccess)
        {
            return OperationResult<ViaGuestName>.Failure(lastNameResult.OperationErrors);
        }

        return OperationResult<ViaGuestName>.Success(new ViaGuestName(firstNameResult.Payload, lastNameResult.Payload));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
    }
}