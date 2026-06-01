using AXIOCRM.Application.Clients.Queries;
using AXIOCRM.Application.Orders.Queries;
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
    public class GetClientsQueryHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnAllClients_WhenClientsExist()
        {
            // Arrange Organiser 
            var options = GetDbOptions();
            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using (var arrangeContext = new AppDbContext(options))
            {
                var clients = new List<Client>
                {
                  new Client { Name = "oumaima", LastName="BA", Email="oumaima@gmail.com", Phone="21548752" },
                  new Client { Name = "ameni", LastName="BA", Email="ameni@gmail.com", Phone="98654123"}
                };

                arrangeContext.Clients.AddRange(clients);
                await arrangeContext.SaveChangesAsync();
            }

            // Act
            using (var actContext = new AppDbContext(options))
            {
                var handler = new GetClientsQueryHandler(actContext);
                var result = await handler.HandleAsync();

                // Assert
                result.Should().NotBeNull();

            }

            suppressTransaction.Complete();
        }
    }
  
}
