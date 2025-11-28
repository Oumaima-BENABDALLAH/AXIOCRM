using ProductManager.API.Models.Invoice;

namespace ProductManager.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; } 

        public DateTime OrderDate { get; set; }

        public string PaymentMethod { get; set; } = "";
        // if case
        public decimal? CashAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? DeliveryMethodId { get; set; }
        // if card
        public string? CardNumber { get; set; }
        public string? CardHolder { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }
        public decimal TotalAmount { get; set; }

       
    
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
