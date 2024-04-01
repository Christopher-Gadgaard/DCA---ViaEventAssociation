using Moq;
using UnitTests.Common.Factories;
using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Guest.CancelParticipation;

public class GuestCancelsParticipationHandlerTest
{
    private readonly FakeUnitOfWork _fakeUnitOfWork;
    
    [Fact]
    public  async Task GivenNothing_WhenCreatingHandler_Success()
    {
        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var eventId = ViaEventId.Create().Payload;
        var startDate = DateTime.Now.AddDays(1);
        var endDate = startDate.AddHours(2);
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).WithDateTimeRange(startDate,endDate).WithGuests(new List<ViaGuestId>{guest.Id}).Build();

        Assert.Single(viaEvent.Guests);
        Assert.Equal(viaEvent.Guests.First(), guest.Id);
        Assert.True(viaEvent.IsParticipant(guest.Id));
        Assert.Contains(guest.Id, viaEvent.Guests);
        var guestRepo = new FakeGuestRepository();
        // var eventRepo = new FakeEventRepository();
        var eventRepo = new Mock<IViaEventRepository>(); 
        eventRepo.Setup( x=> x.GetByIdAsync(It.IsAny<ViaEventId>())).Returns(Task.FromResult(viaEvent));
        var command = GuestCancelsParticipationCommand.Create(viaEvent.Id.Value.ToString(), guest.Id.Value.ToString());
        
        Assert.True(command.IsSuccess);
        
        var handler = new GuestCancelsParticipationHandler(guestRepo, eventRepo.Object, _fakeUnitOfWork);
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        var result = await handler.Handle(command.Payload);    
        
        Assert.True(result.IsSuccess);
    }
}