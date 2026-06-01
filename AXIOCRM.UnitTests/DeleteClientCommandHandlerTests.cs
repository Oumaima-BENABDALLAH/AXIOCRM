using AXIOCRM.Application.Clients.Commands.DeleteClient;
using AXIOCRM.Application.Orders.Commands.DeleteOrder;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace AXIOCRM.UnitTests
{
    public class DeleteClientCommandHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldDeleteClientAndItsProducts_WhenOrClientExists()
        {

            var options = GetDbOptions();
            int generatedId;

            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);
            using (var arrangeContext = new AppDbContext(options))
            {
                var clientToDelete = new Client
                {
                    Name = "John",
                    LastName = "Doe",
                    Email = "",
                    Phone = "",
                    Address = "",
                    City = "",
                    Province = "",
                    PostalCode = "",
                    Country = "",


                };

                arrangeContext.Clients.Add(clientToDelete);
                await arrangeContext.SaveChangesAsync();
                generatedId = clientToDelete.Id;
            }

            // Act
            using (var actContext = new AppDbContext(options))
            {
                var handler = new DeleteClientCommandHandler(actContext);
                var isDeleted = await handler.HandleAsync(generatedId);

                // Assert
                isDeleted.Should().BeTrue();
            }

            // Vérification de la purge en base
            using (var verifyContext = new AppDbContext(options))
            {
                var clientInDb = await verifyContext.Clients.FindAsync(generatedId);
                clientInDb.Should().BeNull();
            }

            suppressTransaction.Complete();

        }
        [Fact]
        public async Task HandleAsync_ShouldReturnFalse_WhenClientDoesNotExist()
        {
            // Arrange
            var options = GetDbOptions();
            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using var context = new AppDbContext(options);
            var handler = new DeleteClientCommandHandler(context);

            // Act
            var isDeleted = await handler.HandleAsync(999); // ID inexistant

            // Assert
            isDeleted.Should().BeFalse();

            suppressTransaction.Complete();
        }
    }
}
