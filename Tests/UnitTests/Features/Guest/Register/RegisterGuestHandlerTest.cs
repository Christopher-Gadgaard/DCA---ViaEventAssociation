using Moq;
using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Application.Features.Guest;
using ViaEventAssociation.Core.Tools.OperationResult.OperationError;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.Register;

public class RegisterGuestHandlerTest
{
    [Fact]
    public async Task GivenValidCommand_WhenHandlingCommand_Success()
    {
        //Arrange
        var emailChecker = new Mock<ICheckEmailInUse>();
        var command = RegisterGuestCommand.Create("Vlad", "Lazar", "308826@via.dk", emailChecker.Object).Payload;

        Assert.Equal("308826@via.dk", command.Guest.ViaEmail.Value);
        IUnitOfWork unitOfWork = new FakeUnitOfWork();
        var guestRepository = new FakeGuestRepository();
        guestRepository.AddGuest(command.Guest);
        ICommandHandler<RegisterGuestCommand> handler = new RegisterGuestHandler(guestRepository, unitOfWork);
        //Act
        var result = await handler.HandleAsync(command);
        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("308826@via.dk", guestRepository.GetByIdAsync(command.Guest.Id).Result?.ViaEmail.Value);
    }


}