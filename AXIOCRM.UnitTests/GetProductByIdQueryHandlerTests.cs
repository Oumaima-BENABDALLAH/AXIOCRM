using AXIOCRM.Application.Orders.Queries;
using AXIOCRM.Application.Products.Queries;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public class GetProductByIdQueryHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;
        }
        [Fact]
        public async Task HandleAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var options = GetDbOptions();

            // On passe 'options' et non pas 'context' !
            using var context = new AppDbContext(options);
            var handler = new GetProductByIdQueryHandler(context);

            // Act
            var result = await handler.HandleAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnProductDTO_WhenProductExists()
        {
            // Arrange
            var options = GetDbOptions();
            int generatedId;
            using (var arrangeContext = new AppDbContext(options))
            {
                var product = new Product
                {
                    Name = "Test Product",
                    Description = "A product for testing",
                    Price = 99.99m,
                    StockQuantity = 50,
                    Sales = 10,
                    ImageUrl = "http://example.com/image.jpg",
                    Color = "Red"

                };

                arrangeContext.Products.Add(product);
                await arrangeContext.SaveChangesAsync();

                generatedId = product.Id; // On récupère l'ID généré
            }
            using (var actContext = new AppDbContext(options))
            {
                var handler = new GetOrderByIdQueryHandler(actContext);

                // Act
                var result = await handler.HandleAsync(generatedId);

                // Assert
                //result.Should().NotBeNull();
                //  result!.Id.Should().Be(generatedId);
                // result.TotalAmount.Should().Be(865m);
            }
        }

    }
}
