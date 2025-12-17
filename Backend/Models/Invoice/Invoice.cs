namespace ProductManager.API.Models.Invoice
{
    public class Invoice
    {
        public int Id { get; set; }

        // Relation vers Order
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        // Numérotation
        public string InvoiceNumber { get; set; } = string.Empty;

        // Dates
        public DateTime IssueDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }

        // Totaux
        public decimal SubTotal { get; set; }
        public decimal TaxRate { get; set; } = 0.19m;   // TVA 19% par exemple
        public decimal TaxAmount { get; set; }
        public decimal Total { get; set; }

        // Status (enum)
        public InvoiceStatus Status { get; set; } = InvoiceStatus.Draft;

        // Notes optionnelles
        public string? Notes { get; set; }

        // Items liés (si tu veux une facture détaillée)
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
