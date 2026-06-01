using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Products.Commands.CreateProduct
{
    public record  CreateProductCommand
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int StockQuantity { get; init; }
        public string? ImageUrl { get; init; }
        public string? Color { get; init; }

    }
}
