using AXIOCRM.Domain.Entities.Invoice;

namespace AXIOCRM.Application.Interfaces
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
