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
    public class GetAllProductsQueryHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnAlProducts_WhenProductsExist()
        {
            // Arrange Organiser 
            var options = GetDbOptions();
            using var suppressTransaction = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled);

            using (var arrangeContext = new AppDbContext(options))
            {
                var products = new List<Product>
                {
                  new Product { Name = "Product 1", Description = "Desc 1", Price = 10m, StockQuantity = 100, ImageUrl = "http://img1", Color = "Red" },
                  new Product { Name = "Product 2", Description = "Desc 2", Price = 20m, StockQuantity = 200, ImageUrl = "http://img2", Color = "Blue" }
                };

                arrangeContext.Products.AddRange(products);
                await arrangeContext.SaveChangesAsync();
            }

            // Act
            using (var actContext = new AppDbContext(options))
            {
                var handler = new GetOrdersQueryHandler(actContext);
                var result = await handler.HandleAsync();

                // Assert
                result.Should().NotBeNull();
               
            }

            suppressTransaction.Complete();
        }
    }
}

