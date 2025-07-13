using ProductManager.API.Models;

namespace ProductManager.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(int id);
        Task<Order> CreateAsync(Order order);
        Task<decimal> GetTotalEarningsAsync();
        Task<decimal> GetTotalBalanceAsync();
        Task<int> GetTotalProjectsAsync();

    }
}
