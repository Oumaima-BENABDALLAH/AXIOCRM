namespace ProductManager.API.Models
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public int Sales { get; set; }
        public string? ImageUrl { get; set; }
        public string? Color { get; set; }

        public decimal Revenue => Sales * Price;
        public string Status
        {
            get
            {
                if (StockQuantity == 0)
                    return "Out of Stock";
                if (StockQuantity < 5)
                    return "Restock";
                return "In Stock";
            }
        }

      
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();


    }
}
