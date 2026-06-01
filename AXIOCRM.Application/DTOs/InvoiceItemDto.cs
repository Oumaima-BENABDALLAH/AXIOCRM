using AXIOCRM.Domain.Entities.Invoice;

namespace AXIOCRM.Application.DTOs
{
    public class InvoiceItemDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }


        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal Total => UnitPrice * Quantity;
    }
}
