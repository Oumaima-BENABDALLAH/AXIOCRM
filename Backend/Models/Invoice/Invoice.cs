namespace ProductManager.API.Models.Invoice
{
    public class Invoice
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }

        public decimal SubTotal { get; set; }
        public decimal TaxRate { get; set; } = 0.19m;   
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }

        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;
        public string? Notes { get; set; }
        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }

    public enum InvoiceStatus
    {
        Draft,
        Sent,
        Paid,
        Overdue
    }

}
