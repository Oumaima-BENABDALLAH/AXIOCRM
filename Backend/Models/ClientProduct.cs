namespace ProductManager.API.Models
{
    public class ClientProduct
    {
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
