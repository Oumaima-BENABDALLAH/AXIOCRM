namespace ProductManager.API.Models.dto
{
    public class CreateInvoiceDto
    {
        public int OrderId { get; set; }

        public string? Notes { get; set; }

        public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }
    public class CreateInvoiceItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
    public class UpdateInvoiceStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }

}
