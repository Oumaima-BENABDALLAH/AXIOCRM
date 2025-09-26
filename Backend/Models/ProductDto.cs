namespace ProductManager.API.Models
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int Sales { get; set; }
        public decimal Revenue { get; set; }
        public string Status { get; set; }

    }
}
