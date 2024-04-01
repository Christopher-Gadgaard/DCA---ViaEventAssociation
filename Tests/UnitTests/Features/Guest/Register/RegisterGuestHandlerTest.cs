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
        IViaGuestRepository guestRepository = new FakeGuestRepository();
        ICommandHandler<RegisterGuestCommand> handler = new RegisterGuestHandler(guestRepository, unitOfWork);
        //Act
        var result = await handler.Handle(command);
        //Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("308826@via.dk", guestRepository.GetByIdAsync(command.Guest.Id).Result?.ViaEmail.Value);
    }

    // [Fact]
    // public async Task RegisterGuestRepoFail()
    // {
    //     var emailChecker = new Mock<ICheckEmailInUse>();
    //     var command = RegisterGuestCommand.Create("Vlad", "Lazar", "308826@via.dk", emailChecker.Object).Payload;
    //
    //    
    //     IUnitOfWork unitOfWork = new FakeUnitOfWork();
    //     var guestRepository = new Mock<IViaGuestRepository>();
    //     guestRepository.Setup(x => x.AddAsync(It.IsAny<ViaGuest>())).Returns(Task.FromResult<OperationResult>(OperationResult.Failure(new List<OperationError>{new(ErrorCode.InternalServerError, "Error while adding guest")})));
    //     ICommandHandler<RegisterGuestCommand> handler = new RegisterGuestHandler(guestRepository.Object, unitOfWork);
    //     
    //     Assert.NotNull(handler);
    //     //Act
    //     var result = await handler.Handle(command);
    //     
    //     //Assert
    //     Assert.True(result.IsFailure);
    // }
}