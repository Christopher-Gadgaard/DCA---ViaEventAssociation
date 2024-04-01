using Moq;
using UnitTests.Common.Factories;
using UnitTests.Fakes;
using UnitTests.Fakes.Repositories;
using UnitTests.Features.Event;
using Via.EventAssociation.Core.Domain.Aggregates.Event.Enums;
using Via.EventAssociation.Core.Domain.Aggregates.Guests;
using Via.EventAssociation.Core.Domain.Common.UnitOfWork;
using Via.EventAssociation.Core.Domain.Common.Values.Ids;
using ViaEventAssociation.Core.AppEntry.Commands;
using ViaEventAssociation.Core.AppEntry.Commands.Event;
using ViaEventAssociation.Core.Application.Features.Event;

namespace UnitTests.Features.Guest.Participate;

public class GuestParticipationHandlerTest
{
    private readonly IUnitOfWork _unitOfWork= new FakeUnitOfWork();

    [Fact]
    public async Task GivenNothing_WhenParticipatingToEvent_Success()
    {
        var eventId = ViaEventId.Create().Payload;
        var viaEvent = ViaEventTestDataFactory.Init(eventId).WithStatus(ViaEventStatus.Active)
            .WithVisibility(ViaEventVisibility.Public).Build();

        var guest = ViaGuestTestFactory.CreateValidViaGuest();
        var guestRepo = new FakeGuestRepository();
        var eventRepo = new FakeEventRepository();
        eventRepo.AddEvent(viaEvent);
        var command = GuestParticipateCommand.Create(viaEvent.Id.Value, guest.Id.Value);
        ICommandHandler<GuestParticipateCommand> handler =
            new GuestParticipateHandler(eventRepo, guestRepo, _unitOfWork);
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        Assert.NotNull(command.Payload);
        Assert.Equal(viaEvent.Id.Value, command.Payload.EventId.Value);
        Assert.Equal(guest.Id, command.Payload.GuestId);

    var result = await handler.Handle(command.Payload);
    Assert.True(result.IsSuccess);
    }
    //
    // [Fact]
    // public async Task GivenNothing_WhenParticipatingToEvent_Fail()
    // {
    //     var eventId = ViaEventId.Create().Payload;
    //     var viaEvent = ViaEventTestDataFactory.Init(eventId)
    //         .WithVisibility(ViaEventVisibility.Public).Build();
    //
    //     var guest = ViaGuestTestFactory.CreateValidViaGuest();
    //     var guestRepo = new Mock<IViaGuestRepository>();
    //     var eventRepo = new FakeEventRepository();
    //     var command = GuestParticipateCommand.Create(viaEvent.Id.Value, guest.Id.Value);
    //     ICommandHandler<GuestParticipateCommand> handler =
    //         new GuestParticipateHandler(eventRepo, guestRepo, _unitOfWork);
    //     if (handler == null) throw new ArgumentNullException(nameof(handler));
    //
    //     Assert.NotNull(command.Payload);
    //     Assert.Equal(viaEvent.Id.Value, command.Payload.EventId.Value);
    //     Assert.Equal(guest.Id, command.Payload.GuestId);
    //
    //     var result = await handler.Handle(command.Payload);
    //     Assert.True(result.IsFailure);
    // }
}