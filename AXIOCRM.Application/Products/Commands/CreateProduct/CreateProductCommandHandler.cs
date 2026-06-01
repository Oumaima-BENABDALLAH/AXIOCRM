using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Products.Commands.CreateProduct
{
    public  class CreateProductCommandHandler
    {
        private readonly AppDbContext _context;

        public CreateProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AXIOCRM.Application.DTOs.ProductDTO> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                throw new ArgumentException("Name is required.", nameof(command.Name));

            if (command.Price < 0) throw new ArgumentException("Price must be non-negative.", nameof(command.Price));
            if (command.StockQuantity < 0) throw new ArgumentException("StockQuantity must be non-negative.", nameof(command.StockQuantity));

            var product = new Product
            {
                Name = command.Name.Trim(),
                Description = command.Description?.Trim() ?? string.Empty,
                Price = command.Price,
                StockQuantity = command.StockQuantity,
                ImageUrl = command.ImageUrl,
                Color = command.Color,
                Sales = 0
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.ToDto();
        }
    }
}