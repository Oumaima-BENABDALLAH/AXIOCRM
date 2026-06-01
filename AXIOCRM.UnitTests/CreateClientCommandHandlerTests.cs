using AXIOCRM.Application.Clients.Commands.CreateClient;
using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Products.Commands.CreateProduct;
using AXIOCRM.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AXIOCRM.UnitTests
{
    public class CreateClientCommandHandlerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }


        [Fact]
        public async Task HandleAsync_ShouldCreateClient()
        {
            using var context = GetDbContext();

            var command = new CreateClientCommand
            (
                "Oumaima", "BA", new DateTime(1990, 1, 1), "Software Engineer",
                "Active", "profile.jpg", "oumaima@gmail.com", "1234567890",
                "123 Main", "City", "Province", "12345", "Country", "IT",
                "Developer", new DateTime(2020, 1, 1), 60000, "REF123",
                "Group", "Manager", "Full-Time", "Notes", "Division"
            );

           
            var handler = new CreateClientCommandHandler(context);
            var result = await handler.HandleAsync(command);


            result.Should().NotBeNull();
          
            result.Name.Should().Be("Oumaima");
            var clientInDb = await context.Clients.FindAsync(result.Id);
            clientInDb.Should().NotBeNull();
            clientInDb!.Name.Should().Be("Oumaima");
        }
    }
}
