namespace ProductManager.API.Models.dto
{
    public class InvoiceDto
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }

        public string Status { get; set; } = string.Empty;

        public string? Notes { get; set; }
        public OrderDto? Order { get; set; }
        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
