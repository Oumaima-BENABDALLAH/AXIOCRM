using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AXIOCRM.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler
    {
        private readonly AppDbContext _context;

        public UpdateOrderCommandHandler(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OrderDto?> HandleAsync(UpdateOrderCommand command)
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

                // 3. RÉCUPÉRATION DE L'AGRÉGAT (SANS AsNoTracking pour la modification)
                var existingOrder = await _context.Orders
                    .Include(o => o.OrderProducts)
                    .FirstOrDefaultAsync(o => o.Id == command.Id);

                if (existingOrder == null)
                {
                    return null;
                }
                // 4. MISE À JOUR DES PROPRIÉTÉS DE BASE
                existingOrder.ClientId = command.ClientId;
                existingOrder.OrderDate = command.OrderDate;
                existingOrder.PaymentMethod = command.PaymentMethod;
                existingOrder.CashAmount = command.CashAmount;
                existingOrder.PaymentDate = command.PaymentDate;
                existingOrder.DeliveryMethodId = command.DeliveryMethodId;
                existingOrder.CardNumber = command.CardNumber;
                existingOrder.CardHolder = command.CardHolder;
                existingOrder.ExpiryDate = command.ExpiryDate;
                existingOrder.CVV = command.CVV;

                // 5. NETTOYAGE DES ANCIENS PRODUITS (Cascade en mémoire)
                if (existingOrder.OrderProducts != null && existingOrder.OrderProducts.Any())
                {
                    _context.OrderProducts.RemoveRange(existingOrder.OrderProducts);
                }

                // 6. INSERTION DES NOUVEAUX PRODUITS ET CALCUL DU TOTAL
                decimal total = 0m;

                if (command.OrderProducts != null && command.OrderProducts.Any())
                {
                    // Tri par ProductId pour éviter les verrous croisés (Deadlocks)
                    var sortedProducts = command.OrderProducts.OrderBy(p => p.ProductId).ToList();

                    foreach (var p in sortedProducts)
                    {
                        var newOp = new OrderProduct
                        {
                            OrderId = existingOrder.Id,
                            ProductId = p.ProductId,
                            Quantity = p.Quantity,
                            UnitPrice = p.UnitPrice,
                            Color = p.Color,
                            ImageUrl = p.ImageUrl
                        };

                        total += newOp.Quantity * newOp.UnitPrice;
                        _context.OrderProducts.Add(newOp);
                    }
                }
                existingOrder.TotalAmount = total;

                // Sauvegarde de toutes les modifications d'un coup
                await _context.SaveChangesAsync();

                // 7. COMMIT : On valide définitivement la transaction
                scope.Complete();

                // On recharge l'entité avec ses nouvelles relations pour le DTO de retour
                existingOrder.OrderProducts = await _context.OrderProducts
                    .Include(op => op.Product)
                    .Where(op => op.OrderId == existingOrder.Id)
                    .ToListAsync();

                return existingOrder.ToDto();


            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                throw; // Rethrow to be handled by upper layers
            }


        }
     }
}
