using ProductManager.API.Models;

namespace ProductManager.API.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllAsync();
        Task<Client> GetByIdAsync(int id);
        Task<Client> GetClientWithOrdersAsync(int id);
        Task<Client> CreateAsync(Client client);
        Task<Client> UpdateAsync(int id, Client client);
        Task<bool> DeleteAsync(int id);
    }
}
