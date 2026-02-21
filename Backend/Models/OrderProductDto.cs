namespace ProductManager.API.Models
{
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = "";
        public int OrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }

    }
}
