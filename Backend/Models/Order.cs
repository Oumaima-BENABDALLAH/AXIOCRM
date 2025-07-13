namespace ProductManager.API.Models
{
    public class Order
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public DateTime OrderDate { get; set; }

        public string PaymentMethod { get; set; } = "";
        public decimal TotalAmount { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
