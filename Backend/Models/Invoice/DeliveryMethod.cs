namespace ProductManager.API.Models.Invoice
{
    public class DeliveryMethod
    {
        public int Id { get; set; }
        public string Name { get; set; } // Ex: "Standard", "Express", "Pickup"
        public decimal Price { get; set; } // Ex: 10.00 €
        public int EstimatedDays { get; set; } // Ex: 2-3 jours
    }
}
