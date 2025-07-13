namespace ProductManager.API.Models
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        // public ICollection<ClientProduct> ClientProducts { get; set; } = new List<ClientProduct>();
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();


    }
}
