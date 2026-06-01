using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Application.Orders.Commands.UpdateOrder;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AXIOCRM.Application.Products.Commands.UpdateProduct
{
    public  class UpdateProductCommandHandler
    {
        private readonly AppDbContext _context;

        public UpdateProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDTO?> HandleAsync(UpdateProductCommand command)
        {
            // 1. DÉFINITION DE L'ISOLATION POUR LA LECTURE
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(15)
            };

            // 2. BEGIN TRANSACTION (Lecture seule)
            using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled);

            try
            {

                var product = await _context.Products
                                .FirstOrDefaultAsync(p => p.Id == command.Id);

                if (product == null) return null;

                product.Name = command.Name;
                product.Description = command.Description;
                product.Price = command.Price;
                product.StockQuantity = command.StockQuantity;
                product.ImageUrl = command.ImageUrl;
                product.Color = command.Color;
                await _context.SaveChangesAsync();
                scope.Complete();

                return product.ToDto();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw; // Rethrow the exception to be handled by the caller
            }
        }
    
    }
}
