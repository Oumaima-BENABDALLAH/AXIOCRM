using AXIOCRM.Application.Orders.Commands.UpdateOrder;
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
    public class UpdateOrderCommandHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldUpdateOrderAndReplaceProducts_WhenCommandIsValid()
        {
            // Arrange
            var options = GetDbOptions();
            int generatedId;

            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using (var arrangeContext = new AppDbContext(options))
            {
                var originalOrder = new Order
                {
                    ClientId = 5,
                    OrderDate = DateTime.UtcNow,
                    PaymentMethod = "Virement",
                    TotalAmount = 500m,
                    OrderProducts = new List<OrderProduct>
                    {
                        new OrderProduct { ProductId = 1, Quantity = 1, UnitPrice = 500m }
                    }
                };

                arrangeContext.Orders.Add(originalOrder);
                await arrangeContext.SaveChangesAsync();
                generatedId = originalOrder.Id;
            }

            var command = new UpdateOrderCommand
            {
                Id = generatedId,
                ClientId = 10, // Nouveau client
                OrderDate = DateTime.UtcNow,
                PaymentMethod = "Cash", // Nouveau moyen
                OrderProducts = new List<UpdateOrderProductItem>
                {
                    new UpdateOrderProductItem { ProductId = 2, Quantity = 2, UnitPrice = 100m } // Nouveau produit
                }
            };

            // Act
            using (var actContext = new AppDbContext(options))
            {
                var handler = new UpdateOrderCommandHandler(actContext);
                var result = await handler.HandleAsync(command);

                // Assert
                result.Should().NotBeNull();
                result!.Id.Should().Be(generatedId);
                result.ClientId.Should().Be(10);
                result.PaymentMethod.Should().Be("Cash");
                result.TotalAmount.Should().Be(200m); // 2 * 100m
            }

            // Vérification finale directe dans la base de données InMemory
            using (var verifyContext = new AppDbContext(options))
            {
                var orderInDb = await verifyContext.Orders
                    .Include(o => o.OrderProducts)
                    .FirstOrDefaultAsync(o => o.Id == generatedId);

                orderInDb.Should().NotBeNull();
                orderInDb!.OrderProducts.Should().HaveCount(1);
                orderInDb.OrderProducts.Should().Contain(op => op.ProductId == 2);
            }

            suppressTransaction.Complete();
        }
    }
}