using ProductManager.API.Models;
using ProductManager.API.Models.Invoice;

namespace ProductManager.API.Services.Interfaces
{
    public interface IDeliveryMethod
    {
        Task<IEnumerable<DeliveryMethod>> GetAllAsync();
        Task<DeliveryMethod> GetByIdAsync(int id);
        Task<DeliveryMethod> CreateAsync(DeliveryMethod deliveryMethod);
        Task<DeliveryMethod> UpdateAsync(int id, DeliveryMethod deliveryMethod);
        Task<bool> DeleteAsync(int id);
    }
}
