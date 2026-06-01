using AXIOCRM.Application.Clients.Commands.UpdateClient;
using AXIOCRM.Application.Products.Commands.UpdateProduct;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public  class UpdateClientCommandHandlerTests
    {
        private DbContextOptions<AppDbContext> GetDbOptions()
        {
            return new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task HandleAsync_ShouldReturnNull_WhenClientDoesNotExist()
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
        public async Task HandleAsync_ShouldUpdateClient_WhenClientExists()
        {
            var options = GetDbOptions();

            using (var context = new AppDbContext(options))
            {
                context.Clients.Add(new Client
                {
                    Id = 1,
                    Name = "Ancien Nom",
                    LastName = "Ancien Prénom",
                    Email = "",
                    Phone = "",
                    Address = "",
                    City = "",
                    Province = "",

                });
                await context.SaveChangesAsync();
            }

            var command = new UpdateClientCommand
            {
                Id = 1,
                Name = "Nouveau Nom",
                LastName = "Nouveau Prénom",
                Email = "",
                Phone = "",
                Address = "",
                City = "",
                Province = "",
            };
            using (var context = new AppDbContext(options))
            {
                var handler = new UpdateClientCommandHandler(context);
                var result = await handler.HandleAsync(command);
            }

            using (var context = new AppDbContext(options))
            {
                var updatedClient = await context.Clients.FindAsync(1);

                Assert.NotNull(updatedClient.Id);
                Assert.Equal("Nouveau Nom", updatedClient.Name);
                
            }

        }

    }
}
