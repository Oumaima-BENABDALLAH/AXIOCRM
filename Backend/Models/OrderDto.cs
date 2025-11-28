using ProductManager.API.Models.Invoice;

namespace ProductManager.API.Models
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }

        public string PaymentMethod { get; set; } = "";

        public decimal? CashAmount { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? DeliveryMethodId { get; set; }

        // Card payment
        public string? CardNumber { get; set; }
        public string? CardHolder { get; set; }
        public string? ExpiryDate { get; set; }
        public string? CVV { get; set; }

        public decimal? TotalAmount { get; set; }

        public ClientDto? Client { get; set; }

        public List<OrderProductDto> OrderProducts { get; set; } = new();
    }
}
