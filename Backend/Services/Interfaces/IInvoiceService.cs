using ProductManager.API.Models.Invoice;

namespace ProductManager.API.Services.Interfaces
{
    public interface  IInvoiceService
    {
        Task<Invoice> GenerateInvoiceFromOrderAsync(int orderId);
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
    }
}
