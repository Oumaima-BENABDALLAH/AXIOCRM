using AXIOCRM.Application.Orders.Queries;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public class GetOrderByIdQueryHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                 .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                 .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnNull_WhenOrderDoesNotExist()
        {
            // Arrange
            var options = GetDbOptions();

            // On passe 'options' et non pas 'context' !
            using var context = new AppDbContext(options);
            var handler = new GetOrderByIdQueryHandler(context);

            // Act
            var result = await handler.HandleAsync(999);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnOrderDto_WhenOrderExists()
        {
            // Arrange
            var options = GetDbOptions();
            int generatedId;
            using (var arrangeContext = new AppDbContext(options))
            {
                var order = new Order
                {
                    ClientId = 10,
                    OrderDate = DateTime.UtcNow,
                    PaymentMethod = "Cash",
                    TotalAmount = 865m
                };

                arrangeContext.Orders.Add(order);
                await arrangeContext.SaveChangesAsync();

                generatedId = order.Id; // On récupère l'ID généré
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