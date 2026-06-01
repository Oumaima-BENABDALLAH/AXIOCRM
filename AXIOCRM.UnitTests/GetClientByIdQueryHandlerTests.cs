using AXIOCRM.Application.Clients.Queries;
using AXIOCRM.Application.Orders.Queries;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public class GetClientByIdQueryHandlerTests
    {

        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;
        }


        [Fact]
        public async Task HandleAsync_ShouldReturnNull_WhenClientDoesNotExist()
        {
            // Arrange
            var options = GetDbOptions();

            using var context = new AppDbContext(options);
            var handler = new GetClientByIdQueryHandler(context);

            // Act
            var result = await handler.HandleAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnClientDto_WhenClientExists()
        {
            // Arrange
            var options = GetDbOptions();
            int generatedId;
            using (var arrangeContext = new AppDbContext(options))
            {
                var client = new Client
                {
                    Name = "John",
                    LastName = "Doe",
                    Email = "",
                    Phone = "",
                };

                arrangeContext.Clients.Add(client);
                await arrangeContext.SaveChangesAsync();

                generatedId = client.Id; // On récupère l'ID généré
            }
            using (var actContext = new AppDbContext(options))
            {
                var handler = new GetClientByIdQueryHandler(actContext);

                // Act
                var result = await handler.HandleAsync(generatedId);

                // Assert
                result.Should().NotBeNull();
                result!.Id.Should().Be(generatedId);
            }
        }

    }
}
