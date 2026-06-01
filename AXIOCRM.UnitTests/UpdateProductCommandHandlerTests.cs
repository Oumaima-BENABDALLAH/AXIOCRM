using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Products.Commands.UpdateProduct;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public class UpdateProductCommandHandlerTests
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
            // ARRANGE
            var options = GetDbOptions();
            var command = new UpdateProductCommand { Id = 999 }; // ID inexistant
            
            // ACT
            using var context = new AppDbContext(options);
            var handler = new UpdateProductCommandHandler(context);
            var result = await handler.HandleAsync(command);

            // ASSERT
            Assert.Null(result);
        }

        [Fact]
        public async Task HandleAsync_ShouldUpdateProduct_WhenProductExists()
        {
            var options = GetDbOptions();

            using (var context = new AppDbContext(options))
            {
                context.Products.Add(new Product
                {
                    Id = 1,
                    Name = "Ancien Nom",
                    Price = 15,
                    StockQuantity = 5
                });
                await context.SaveChangesAsync();
            }

            var command = new UpdateProductCommand 
            {
                Id = 1,
                Name = "Nouveau Nom",
                Price = 15,
                StockQuantity = 10,
                Description = "Nouvelle Description",
            };

            // action

            ProductDTO result;
            using (var context = new AppDbContext(options))
            {
                var handler = new UpdateProductCommandHandler(context);
                result = await handler.HandleAsync(command);
            }

            // ASSERT
            using (var context = new AppDbContext(options))
            {
                var updatedProduct = await context.Products.FindAsync(1);

                Assert.NotNull(result);
                Assert.Equal("Nouveau Nom", result.Name);
                Assert.Equal(15, result.Price);

                // Vérification en base de données
                Assert.NotNull(updatedProduct);
                Assert.Equal("Nouveau Nom", updatedProduct.Name);
                Assert.Equal("Nouvelle Description", updatedProduct.Description);
            }
        }
  }
}
