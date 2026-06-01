using AXIOCRM.Application.Services;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace AXIOCRM.UnitTests
{
    public class ClientServiceTests
    {
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
                .Options;

            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }
        #region Create Tests

        [Fact]
        public async Task CreateAsync_ShouldAddClientAndReturnIt()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var service = new ClientService(context);
            var client = new Client
            {
                Name = "John",
                LastName = "Doe",
                Email = "john.doe@axiocrm.com",
                Phone = "123456789"
            };

            // Act
            var result = await service.CreateAsync(client);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal("John Doe", result.FullName);

            var dbClient = await context.Clients.FindAsync(result.Id);
            Assert.NotNull(dbClient);
        }

        #endregion

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllClients()
        {
            // Arrange
            using var context = GetDatabaseContext();
            context.Clients.AddRange(new List<Client>
            {
                new Client { Name = "Alice", LastName = "Smith", Phone = "111111" },
                new Client { Name = "Bob", LastName = "Jones", Phone = "222222" }
            });
            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            var result = await service.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
        [Fact]
        public async Task GetByIdAsync_WithValidId_ShouldReturnClientWithOrders()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var client = new Client
            {
                Id = 1,
                Name = "Jane",
                LastName = "Doe",
                Phone = "987654321",
                Orders = new List<Order> { new Order { Id = 101 } }
            };
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            var result = await service.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane Doe", result.FullName);
            Assert.Single(result.Orders);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var service = new ClientService(context);

            // Act
            var result = await service.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAsync_WhenClientExists_ShouldModifyAndReturnClient()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var client = new Client { Id = 1, Name = "OldName", LastName = "OldLastName", Phone = "000" };
            context.Clients.Add(client);
            await context.SaveChangesAsync();


            context.Entry(client).State = EntityState.Detached;
            var service = new ClientService(context);

            var updatedClient = new Client
            {
                Name = "NewName",
                LastName = "NewLastName",
                Phone = "12345"
            };

            // Act
            var result = await service.UpdateAsync(1, updatedClient);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("NewName NewLastName", result.FullName);

            // On vérifie directement dans le context
            using var verificationContext = GetDatabaseContext();
            var dbClient = await context.Clients.FindAsync(1);
            Assert.Equal("NewName", dbClient.Name);
        }

        [Fact]
        public async Task UpdateAsync_WhenClientDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var service = new ClientService(context);
            var updatedClient = new Client { Name = "New", LastName = "New", Phone = "000" };

            // Act
            var result = await service.UpdateAsync(99, updatedClient);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task DeleteAsync_WhenClientExists_ShouldReturnTrueAndRemoveFromDb()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var client = new Client { Id = 1, Name = "ToDelete", LastName = "User", Phone = "000" };
            context.Clients.Add(client);
            await context.SaveChangesAsync();

            var service = new ClientService(context);

            // Act
            var isDeleted = await service.DeleteAsync(1);

            // Assert
            Assert.True(isDeleted);
            var dbClient = await context.Clients.FindAsync(1);
            Assert.Null(dbClient);
        }

        [Fact]
        public async Task DeleteAsync_WhenClientDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            using var context = GetDatabaseContext();
            var service = new ClientService(context);

            // Act
            var isDeleted = await service.DeleteAsync(99);

            // Assert
            Assert.False(isDeleted);
        }
    }
}
