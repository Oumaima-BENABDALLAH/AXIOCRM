using AXIOCRM.Application.DTOs;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AXIOCRM.Application.Mappings;

namespace AXIOCRM.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler
    {
        private readonly AppDbContext _context;

        public CreateOrderCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto> HandleAsync(CreateOrderCommand command)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TimeSpan.FromSeconds(15)
            };

            using var scope = new TransactionScope(
            TransactionScopeOption.Required,
            transactionOptions,
            TransactionScopeAsyncFlowOption.Enabled);
            try
            {

                var order = new Order
                {
                    ClientId = command.ClientId,
                    OrderDate = command.OrderDate,
                    PaymentMethod = command.PaymentMethod,
                    CashAmount = command.CashAmount,
                    PaymentDate = command.PaymentDate,
                    DeliveryMethodId = command.DeliveryMethodId
                };

                _context.Orders.Add(order);


                await _context.SaveChangesAsync();

                decimal total = 0m;

                if (command.OrderProducts != null)
                {
                    foreach (var p in command.OrderProducts)
                    {

                        var product = await _context.Products
                            .FirstOrDefaultAsync(pr => pr.Id == p.ProductId);

                        if (product == null)
                            throw new Exception($"Produit {p.ProductId} non trouvé.");

                        var price = product.Price;

                        var op = new OrderProduct
                        {
                            OrderId = order.Id,
                            ProductId = p.ProductId,
                            Quantity = p.Quantity,
                            UnitPrice = price
                        };

                        total += price * p.Quantity;
                        _context.OrderProducts.Add(op);
                    }
                }

                // Mise à jour du montant total de la commande
                order.TotalAmount = total;
                await _context.SaveChangesAsync();
                scope.Complete();

                return order.ToDto();
            }
            catch (Exception ex) 
            { 
                throw new Exception($"Erreur lors de la création de la commande : {ex.Message}");
            }
        }
    }
}
