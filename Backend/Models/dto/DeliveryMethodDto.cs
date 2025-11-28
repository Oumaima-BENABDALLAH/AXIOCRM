namespace ProductManager.API.Models.dto
{
    public class DeliveryMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int EstimatedDays { get; set; }
    }
}
