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
    public class DeleteOrderCommandHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldDeleteOrderAndItsProducts_WhenOrderExists()
        {
            // Arrange
            var options = GetDbOptions();
            int generatedId;

            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using (var arrangeContext = new AppDbContext(options))
            {
                var orderToDelete = new Order
                {
                    ClientId = 3,
                    OrderDate = DateTime.UtcNow,
                    PaymentMethod = "Cash",
                    TotalAmount = 1000m,
                    OrderProducts = new List<OrderProduct>
                    {
                        new OrderProduct { ProductId = 9, Quantity = 1, UnitPrice = 1000m }
                    }
                };

                arrangeContext.Orders.Add(orderToDelete);
                await arrangeContext.SaveChangesAsync();
                generatedId = orderToDelete.Id;
            }

            // Act
            using (var actContext = new AppDbContext(options))
            {
                var handler = new DeleteOrderCommandHandler(actContext);
                var isDeleted = await handler.HandleAsync(generatedId);

                // Assert
                isDeleted.Should().BeTrue();
            }

            // Vérification de la purge en base
            using (var verifyContext = new AppDbContext(options))
            {
                var orderInDb = await verifyContext.Orders.FindAsync(generatedId);
                orderInDb.Should().BeNull();

                var productsInDb = await verifyContext.OrderProducts
                    .AnyAsync(op => op.OrderId == generatedId);
                productsInDb.Should().BeFalse();
            }

            suppressTransaction.Complete();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFalse_WhenOrderDoesNotExist()
        {
            // Arrange
            var options = GetDbOptions();
            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using var context = new AppDbContext(options);
            var handler = new DeleteOrderCommandHandler(context);

            // Act
            var isDeleted = await handler.HandleAsync(999); // ID inexistant

            // Assert
            isDeleted.Should().BeFalse();

            suppressTransaction.Complete();
        }
    }
}