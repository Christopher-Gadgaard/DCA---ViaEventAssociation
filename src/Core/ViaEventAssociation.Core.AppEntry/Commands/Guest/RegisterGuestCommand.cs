using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Aggregates.Guests.Values;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using Via.EventAssociation.Core.Domain.Contracts;
using ViaEventAssociation.Core.Tools.OperationResult.OperationResult;

namespace ViaEventAssociation.Core.AppEntry.Commands.Guest;

public class RegisterGuestCommand
{
    public ViaGuest Guest { get;}

    private RegisterGuestCommand(ViaGuest guest)
    {
        Guest = guest;
    }
    
    public static OperationResult<RegisterGuestCommand> Create(string firstName, string lastName, string email, ICheckEmailInUse emailChecker)
    {
   
     OperationResult<ViaGuestName> guestNameResult = ViaGuestName.Create(firstName, lastName);
    OperationResult<ViaEmail> emailResult = ViaEmail.Create(email, emailChecker);
    OperationResult<RegisterGuestCommand> combinedResult =
       
      OperationResult<RegisterGuestCommand>.Combine(guestNameResult.OperationErrors, emailResult.OperationErrors);

    if (combinedResult.IsFailure)
    {
        return OperationResult<RegisterGuestCommand>.Failure(combinedResult.OperationErrors);   
    }
    OperationResult<ViaGuest> guestResult = ViaGuest.Create(ViaGuestId.Create().Payload, guestNameResult, emailResult);
    
    if(guestResult.IsFailure)
    {
        return OperationResult<RegisterGuestCommand>.Failure(guestResult.OperationErrors);
    }

    return OperationResult<RegisterGuestCommand>.Success(new RegisterGuestCommand(guestResult.Payload));
    }
    
  
}