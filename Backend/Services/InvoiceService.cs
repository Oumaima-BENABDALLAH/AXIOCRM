
using ProductManager.API.Data;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Models.Invoice;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;

        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Invoice> GenerateInvoiceFromOrderAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new Exception("Order not found");

            var invoice = new Invoice
            {
                OrderId = order.Id,
                InvoiceNumber = GenerateInvoiceNumber(),
                IssueDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                SubTotal = order.TotalAmount,
                TaxAmount = order.TotalAmount * 0.19m,
                Total = order.TotalAmount * 1.19m
            };

            foreach (var item in order.OrderProducts)
            {
                invoice.Items.Add(new InvoiceItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? string.Empty,
                    UnitPrice = item.Product?.Price ?? 0,
                    Quantity = item.Quantity
                });
            }

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        private string GenerateInvoiceNumber()
        {
            return $"INV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6)}";
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int id)
            => await _context.Invoices
                    .Include(i => i.Items)
                    .Include(i => i.Order)
                    .ThenInclude(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .Include(i => i.Order)
                    .ThenInclude(o => o.Client)
                    .FirstOrDefaultAsync(i => i.Id == id);

        public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync()
            => await _context.Invoices
                    .Include(i => i.Items)
                    .Include(i => i.Order)
                     .ThenInclude(o => o.Client)
                    .ToListAsync();
    }
}