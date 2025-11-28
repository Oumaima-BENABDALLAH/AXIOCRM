using ProductManager.API.Models;

namespace ProductManager.API.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto> GetByIdAsync(int id);
        Task<OrderDto> CreateAsync(OrderDto dto);
        Task<OrderDto> UpdateAsync(int id, OrderDto dto);
        Task<bool> DeleteAsync(int id);

        Task<decimal> GetTotalEarningsAsync();
        Task<decimal> GetTotalBalanceAsync();
        Task<int> GetTotalProjectsAsync();
    }
}
