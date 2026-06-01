using AXIOCRM.Application.Orders.Commands.DeleteOrder;
using AXIOCRM.Application.Products.Commands.DeleteProduct;
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
    public class DeleteProductCommandHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnFalse_WhenProductDoesNotExist()
        {
            // Arrange
            var options = GetDbOptions();
            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using var context = new AppDbContext(options);
            var handler = new DeleteProductCommandHandler(context);

            // Act
            var isDeleted = await handler.HandleAsync(999); // ID inexistant

            // Assert
            isDeleted.Should().BeFalse();

            suppressTransaction.Complete();
        }


        [Fact]
        public async Task HandleAsync_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var options = GetDbOptions();
            int generatedId;

            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using (var arrangeContext = new AppDbContext(options))
            {
                var productToDelete = new Product
                {
                    Name = "Product To Delete",
                    Description = "Description",
                    Price = 100m,
                    StockQuantity = 10,
                    ImageUrl = "http://image.url",
                    Color = "Black"
                
                };

                arrangeContext.Products.Add(productToDelete);
                await arrangeContext.SaveChangesAsync();
                generatedId = productToDelete.Id;
            }

            // Act
            using (var actContext = new AppDbContext(options))
            {
                var handler = new DeleteProductCommandHandler(actContext);
                var isDeleted = await handler.HandleAsync(generatedId);

                // Assert
                isDeleted.Should().BeTrue();
            }

            // Vérification de la purge en base
            using (var verifyContext = new AppDbContext(options))
            {
                var productInDb = await verifyContext.Products.FindAsync(generatedId);
                productInDb.Should().BeNull();

               
            }

            suppressTransaction.Complete();
        }
    }
}
