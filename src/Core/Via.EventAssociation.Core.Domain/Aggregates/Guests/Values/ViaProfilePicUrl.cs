using Via.EventAssociation.Core.Domain.Common.Bases;

namespace Via.EventAssociation.Core.Domain.Aggregates.Guests.Values;

public class ViaProfilePicUrl:ValueObject

{
    public string Value { get; }
    
    private ViaProfilePicUrl(){}
    
    private ViaProfilePicUrl(string value)
    {
        Value = value;
    }
    
    public static ViaProfilePicUrl Create(string profilePicUrl)
    {
        return new ViaProfilePicUrl(profilePicUrl);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}