using Moq;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;

namespace UnitTests.Common.Factories;

public abstract class ViaGuestTestFactory
{
    public static ViaGuest CreateValidViaGuest()
    {
        var viaGuestId = ViaGuestId.Create().Payload;
        var viaGuestName = ViaGuestName.Create("John", "Doe").Payload;
        var email = "308826@via.dk";
        var emailCheckerMock = new Mock<ICheckEmailInUse>();
        emailCheckerMock.Setup(service => service.IsEmailRegistered(email)).Returns(false);
        var viaEmail = ViaEmail.Create(email, emailCheckerMock.Object).Payload;

        return new ViaGuest(viaGuestId, viaGuestName, viaEmail);

    }
}