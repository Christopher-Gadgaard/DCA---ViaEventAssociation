using Moq;
using UnitTests.Common.Factories;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.AppEntry.Commands.Guest;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace UnitTests.Features.Guest.Register;

public class RegisterGuestCommandTest
{
    [Fact]
    public void GivenNothing_WhenCreatingCommand_Success()
    {
        //Arrange
        var emailChecker =new Mock<ICheckEmailInUse>();
        var result=RegisterGuestCommand.Create("Vlad","Lazar", "308826@via.dk",emailChecker.Object);
        
        //Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Payload);
    }

    [Fact]
    public void GivenEmptyFirstName_WhenCreatingCommand_Failure()
    {
        //Arrange
        var emailChecker = new Mock<ICheckEmailInUse>();
        var result = RegisterGuestCommand.Create("", "Lazar", "3088@via.dk", emailChecker.Object);

        //Assert
        Assert.True(result.IsFailure);
    }

    [Fact]
    public void GivenEmptyLastName_WhenCreatingCommand_Failure()
    {
        //Arrange
        var emailChecker = new Mock<ICheckEmailInUse>();
        var result = RegisterGuestCommand.Create("Vlad", "", "308826@via.dk", emailChecker.Object);

        //Assert
        Assert.True(result.IsFailure);
    }
    
    [Fact]
    public void GivenEmptyEmail_WhenCreatingCommand_Failure()
    {
        //Arrange
        var emailChecker = new Mock<ICheckEmailInUse>();
        var result = RegisterGuestCommand.Create("Vlad", "Lazar", "", emailChecker.Object);

        //Assert
        Assert.True(result.IsFailure);
    }
    
}