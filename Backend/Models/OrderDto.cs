namespace ProductManager.API.Models
{
    public class OrderDto
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; } = "";

        public List<OrderProductDto> Products { get; set; } = new();
    }
}
