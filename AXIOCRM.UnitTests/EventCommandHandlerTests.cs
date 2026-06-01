using AXIOCRM.Application.EventScheduler.Commands.CreateEvent;
using AXIOCRM.Application.EventScheduler.Commands.DeleteEvent;
using AXIOCRM.Application.EventScheduler.Commands.UpdateEvent;
using AXIOCRM.Application.EventScheduler.Queries.GetAllEvents;
using AXIOCRM.Application.EventScheduler.Queries.GetEventsById;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Domain.Entities.Scheduler;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public class EventCommandHandlerTests
    {
        private readonly Mock<IIdentityService> _identityMock = new();

        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task Create_ShouldAssignCurrentUserId_WhenUserIsNotAdmin()
        {
            // Arrange
            using var context = GetDbContext();
            var currentUserId = "user-123";
            _identityMock.Setup(x => x.GetCurrentUserIdAsync()).ReturnsAsync(currentUserId);
            _identityMock.Setup(x => x.IsInRoleAsync(currentUserId, "Admin")).ReturnsAsync(false);

            var handler = new CreateEventCommandHandler(context, _identityMock.Object);
            var command = new CreateEventCommand("RDV Client", DateTime.Now, DateTime.Now.AddHours(1), "autre-id");

            // Act
            var result = await handler.HandleAsync(command, CancellationToken.None);

            // Assert
            result.ResourceId.Should().Be(currentUserId); 
            context.ScheduleEvents.Count().Should().Be(1);
        }

        [Fact]
        public async Task Update_ShouldThrowUnauthorized_WhenUserIsNotOwner()
        {
            // Arrange
            using var context = GetDbContext();
            var ownerId = "owner-id";
            var strangerId = "stranger-id";

            var ev = new ScheduleEvent { Id = 1, Title = "Old", ResourceId = ownerId };
            context.ScheduleEvents.Add(ev);
            await context.SaveChangesAsync();

            _identityMock.Setup(x => x.GetCurrentUserIdAsync()).ReturnsAsync(strangerId);
            _identityMock.Setup(x => x.IsInRoleAsync(strangerId, "Admin")).ReturnsAsync(false);

            var handler = new UpdateEventCommandHandler(context, _identityMock.Object);
            var command = new UpdateEventCommand(1, "New Title", DateTime.Now, DateTime.Now, ownerId);

            // Act & Assert
            await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllEvents()
        {
            // Arrange
            using var context = GetDbContext();
            context.ScheduleEvents.AddRange(new List<ScheduleEvent> {
                  new() { Id = 1, Title = "Ev 1", ResourceId = "u1" },
                  new() { Id = 2, Title = "Ev 2", ResourceId = "u1" } });
            await context.SaveChangesAsync();

            var handler = new GetAllEventsQueryHandler(context);

            // Act
            var result = await handler.Handle(new GetAllEventsQuery(), CancellationToken.None);

            // Assert
            result.Should().HaveCount(2);
            result.Any(x => x.Title == "Ev 1").Should().BeTrue();
        }


        [Fact]
        public async Task Handle_ShouldReturnEvent_WhenUserIsOwner()
        {
            using var context = GetDbContext();
            var ownerId = "owner-1";
            var ev = new ScheduleEvent { Id = 1, Title = "Ev", ResourceId = ownerId };
            context.ScheduleEvents.Add(ev);
            await context.SaveChangesAsync();

            var identityMock = new Mock<IIdentityService>();
            identityMock.Setup(x => x.GetCurrentUserIdAsync()).ReturnsAsync(ownerId);
            identityMock.Setup(x => x.IsInRoleAsync(ownerId, "Admin")).ReturnsAsync(false);

            var handler = new GetEventByIdQueryHandler(context, identityMock.Object);
            var result = await handler.Handle(new GetEventByIdQuery(1), CancellationToken.None);

            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.ResourceId.Should().Be(ownerId);
        }

        [Fact]
        public async Task Handle_ShouldReturnEvent_WhenUserIsAdmin()
        {
            using var context = GetDbContext();
            var ownerId = "owner-1";
            var adminId = "admin-1";
            var ev = new ScheduleEvent { Id = 2, Title = "Ev2", ResourceId = ownerId };
            context.ScheduleEvents.Add(ev);
            await context.SaveChangesAsync();

            var identityMock = new Mock<IIdentityService>();
            identityMock.Setup(x => x.GetCurrentUserIdAsync()).ReturnsAsync(adminId);
            identityMock.Setup(x => x.IsInRoleAsync(adminId, "Admin")).ReturnsAsync(true);

            var handler = new GetEventByIdQueryHandler(context, identityMock.Object);
            var result = await handler.Handle(new GetEventByIdQuery(2), CancellationToken.None);

            result.Should().NotBeNull();
            result!.Id.Should().Be(2);
        }

        [Fact]
        public async Task Handle_ShouldThrowUnauthorized_WhenNotOwnerNorAdmin()
        {
            using var context = GetDbContext();
            var ownerId = "owner-1";
            var strangerId = "stranger-1";
            var ev = new ScheduleEvent { Id = 3, Title = "Ev3", ResourceId = ownerId };
            context.ScheduleEvents.Add(ev);
            await context.SaveChangesAsync();

            var identityMock = new Mock<IIdentityService>();
            identityMock.Setup(x => x.GetCurrentUserIdAsync()).ReturnsAsync(strangerId);
            identityMock.Setup(x => x.IsInRoleAsync(strangerId, "Admin")).ReturnsAsync(false);

            var handler = new GetEventByIdQueryHandler(context, identityMock.Object);

            await FluentActions
                .Invoking(() => handler.Handle(new GetEventByIdQuery(3), CancellationToken.None))
                .Should().ThrowAsync<UnauthorizedAccessException>();
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenEventNotFound()
        {
            using var context = GetDbContext();

            var identityMock = new Mock<IIdentityService>();
            identityMock.Setup(x => x.GetCurrentUserIdAsync()).ReturnsAsync("any");
            identityMock.Setup(x => x.IsInRoleAsync(It.IsAny<string>(), "Admin")).ReturnsAsync(false);

            var handler = new GetEventByIdQueryHandler(context, identityMock.Object);
            var result = await handler.Handle(new GetEventByIdQuery(999), CancellationToken.None);

            result.Should().BeNull();
        }

        [Fact]
        public async Task Delete_ShouldRemoveFromDb_WhenSuccessful()
        {
            // Arrange
            using var context = GetDbContext();
            var userId = "user1";
            var ev = new ScheduleEvent { Id = 99, Title = "To Delete", ResourceId = userId };
            context.ScheduleEvents.Add(ev);
            await context.SaveChangesAsync();

            _identityMock.Setup(x => x.GetCurrentUserIdAsync()).ReturnsAsync(userId);
            _identityMock.Setup(x => x.IsInRoleAsync(userId, "Admin")).ReturnsAsync(true); 

            var handler = new DeleteEventCommandHandler(context, _identityMock.Object);

            // Act
            var result = await handler.Handle(new DeleteEventCommand(99), CancellationToken.None);

            // Assert
            result.Should().BeTrue();
            context.ScheduleEvents.Find(99).Should().BeNull();
        }
    }
}
