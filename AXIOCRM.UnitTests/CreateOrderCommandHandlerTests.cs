using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Orders.Commands.CreateOrder;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public class CreateOrderCommandHandlerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task HandleAsync_ShouldCreateOrderAndCalculateTotal_WhenCommandIsValid()
        {
            // Arrange (Préparation des données de test)
            using var context = GetDbContext();

            // On ajoute un produit de test dans notre base InMemory
            var product = new Product { Id = 1, Name = "Laptop", Price = 1000m };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            var handler = new CreateOrderCommandHandler(context);

            var command = new CreateOrderCommand
            {
                ClientId = 1,
                OrderDate = DateTime.UtcNow,
                PaymentMethod = "Carte",
                OrderProducts = new List<CreateOrderProductItem>
                {
                    new CreateOrderProductItem
                    {
                        ProductId = 1,
                        Quantity = 2,
                        UnitPrice = 1000m
                    }
                }
            };

            // Act (Exécution du test)
            OrderDto result = await handler.HandleAsync(command);

            // Assert (Vérification des résultats)
            result.Should().NotBeNull();
            result.Id.Should().BeGreaterThan(0);
            result.TotalAmount.Should().Be(2000m); // 2 * 1000m

            // Vérification en base de données que l'agrégat a bien été enregistré
            var orderInDb = await context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == result.Id);

            orderInDb.Should().NotBeNull();
            orderInDb!.TotalAmount.Should().Be(2000m);
            orderInDb.OrderProducts.Should().HaveCount(1);
        }
    }
}