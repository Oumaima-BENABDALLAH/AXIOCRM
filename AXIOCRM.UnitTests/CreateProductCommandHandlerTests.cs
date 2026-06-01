using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Products.Commands.CreateProduct;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public class CreateProductCommandHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldCreateProductAndReturnDto()
        {
            // Arrange
            var options = GetDbOptions();
            var command = new CreateProductCommand
            {
                Name = "Test product",
                Description = "Desc",
                Price = 12.5m,
                StockQuantity = 10,
                ImageUrl = "http://img",
                Color = "Red"
            };

            // Act
            ProductDTO result;
            using (var context = new AppDbContext(options))
            {
                var handler = new CreateProductCommandHandler(context);
                result = await handler.HandleAsync(command);
            }

            // Assert DTO
            result.Should().NotBeNull();
            result.Name.Should().Be("Test product");
            result.Price.Should().Be(12.5m);
            result.StockQuantity.Should().Be(10);

            // Verify persistence
            using (var verifyContext = new AppDbContext(options))
            {
                var productInDb = await verifyContext.Products.FirstOrDefaultAsync(p => p.Id == result.Id);
                productInDb.Should().NotBeNull();
                productInDb!.Name.Should().Be("Test product");
            }
        }
    }
}
