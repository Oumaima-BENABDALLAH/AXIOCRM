using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AXIOCRM.Application.Orders.Commands.DeleteOrder
{
    public  class DeleteOrderCommandHandler
    {
        private readonly AppDbContext _context;
        public DeleteOrderCommandHandler(AppDbContext context)
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
            
                var existingOrder = await _context.Orders
                    .Include(o => o.OrderProducts)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (existingOrder == null)
                {
                    return false; 
                }

          
                if (existingOrder.OrderProducts != null && existingOrder.OrderProducts.Count > 0)
                {
                    _context.OrderProducts.RemoveRange(existingOrder.OrderProducts);
                }
                _context.Orders.Remove(existingOrder);

                await _context.SaveChangesAsync();

                scope.Complete();

                return true;

            }
            catch (Exception ex)
            {
                // 5. ROLLBACK AUTOMATIQUE en cas de problème
                throw new Exception($"Erreur lors de la suppression de la commande {id}.", ex);

            }
        }
    }
}
