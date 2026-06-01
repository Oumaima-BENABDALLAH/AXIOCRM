using AXIOCRM.Domain.Entities.Invoice;

namespace AXIOCRM.Application.Interfaces
{
    public interface  IInvoiceService
    {
        Task<Invoice> GenerateInvoiceFromOrderAsync(int orderId);
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
    }
}
