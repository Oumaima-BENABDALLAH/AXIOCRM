namespace ProductManager.API.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public ICollection<Order> Orders { get; set; }
        //public ICollection<ClientProduct> ClientProducts { get; set; } = new List<ClientProduct>();

    }
}
