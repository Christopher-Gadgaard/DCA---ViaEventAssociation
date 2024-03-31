using Via.EventAssociation.Core.Domain.Contracts;

namespace UnitTests.Common.Utilities;

public class FakeTimeProvider : ITimeProvider
{
    public DateTime Now { get; private set; }

    public FakeTimeProvider(DateTime fakeNow)
    {
        Now = fakeNow;
    }
}