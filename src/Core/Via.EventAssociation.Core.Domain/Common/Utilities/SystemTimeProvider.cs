using Via.EventAssociation.Core.Domain.Contracts;

namespace Via.EventAssociation.Core.Domain.Common.Utilities;

public class SystemTimeProvider : ITimeProvider
{
    public DateTime Now => DateTime.Now.ToLocalTime();
}