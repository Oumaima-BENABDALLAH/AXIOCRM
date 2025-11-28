namespace ProductManager.API.Models
{
    public class ProductCreateUpdateDto
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int Sales { get; set; }
        public string? Color { get; set; }
        public string? ImageUrl { get; set; }  // optionnel si pas d'upload
        public IFormFile? ImageFile { get; set; }  // fichier uploadé (optionnel)
    }

}
