using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AXIOCRM.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler
    {
        private readonly AppDbContext _context;

        public DeleteProductCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> HandleAsync(int id)
        {
            // 1. DÉFINITION DE L'ISOLATION POUR LA LECTURE
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(10)
            };

            // 2. BEGIN TRANSACTION (Lecture seule)
            using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled);

            try
            {

                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;
                
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                scope.Complete();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la suppression du produit {id}.", ex);
            }  
        
        
        }
     }
}
