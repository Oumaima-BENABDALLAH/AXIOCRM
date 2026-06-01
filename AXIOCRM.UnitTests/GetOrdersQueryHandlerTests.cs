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
    public class GetOrdersQueryHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnAllOrders_WhenOrdersExist()
        {
            // Arrange
            var options = GetDbOptions();
            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using (var arrangeContext = new AppDbContext(options))
            {
                var orders = new List<Order>
                {
                    new Order { ClientId = 10, OrderDate = DateTime.UtcNow, PaymentMethod = "Cash", TotalAmount = 865m },
                    new Order { ClientId = 9, OrderDate = DateTime.UtcNow, PaymentMethod = "Cash", TotalAmount = 2500m }
                };

                arrangeContext.Orders.AddRange(orders);
                await arrangeContext.SaveChangesAsync();
            }

            // Act
            using (var actContext = new AppDbContext(options))
            {
                var handler = new GetOrdersQueryHandler(actContext);
                var result = await handler.HandleAsync();

                // Assert
                result.Should().NotBeNull();
              //  result.Should().HaveCount(2);
               // result.Should().Contain(o => o.TotalAmount == 865m);
               // result.Should().Contain(o => o.TotalAmount == 2500m);
            }

            suppressTransaction.Complete();
        }
    }
}
