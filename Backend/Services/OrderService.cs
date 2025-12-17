using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Hubs;
using ProductManager.API.Models;
using ProductManager.API.Models.dto;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        // -------------------------------
        // GET ALL
        // -------------------------------
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(ToDto).ToList();
        }

        // -------------------------------
        // GET BY ID
        // -------------------------------
        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.OrderProducts).ThenInclude(op => op.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? null : ToDto(order);
        }

        // -------------------------------
        // CREATE
        // -------------------------------
        public async Task<OrderDto> CreateAsync(OrderDto dto)
        {
            var order = new Order
            {
                ClientId = dto.ClientId,
                OrderDate = dto.OrderDate,
                PaymentMethod = dto.PaymentMethod,
                CashAmount = dto.CashAmount,
                PaymentDate = dto.PaymentDate,
                DeliveryMethodId = dto.DeliveryMethodId,
                CardNumber = dto.CardNumber,
                CardHolder = dto.CardHolder,
                ExpiryDate = dto.ExpiryDate,
                CVV = dto.CVV
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            decimal total = 0m;

            if (dto.OrderProducts != null)
            {
                foreach (var p in dto.OrderProducts)
                {
                    var product = await _context.Products.FindAsync(p.ProductId);
                    var price = product?.Price ?? p.UnitPrice;

                    var op = new OrderProduct
                    {
                        OrderId = order.Id,
                        ProductId = p.ProductId,
                        Quantity = p.Quantity,
                        UnitPrice = price,
                        ImageUrl = p.ImageUrl,
                        Color = p.Color
                    };

                    total += price * p.Quantity;

                    _context.OrderProducts.Add(op);
                }
                await _context.SaveChangesAsync();
            }

            order.TotalAmount = total;
            await _context.SaveChangesAsync();

            return ToDto(order);
        }

        // -------------------------------
        // UPDATE
        // -------------------------------
        public async Task<OrderDto> UpdateAsync(int id, OrderDto dto)
        {
            var existing = await _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existing == null) return null;

            // Mettre à jour
            existing.ClientId = dto.ClientId;
            existing.OrderDate = dto.OrderDate;
            existing.PaymentMethod = dto.PaymentMethod;
            existing.CashAmount = dto.CashAmount;
            existing.PaymentDate = dto.PaymentDate;
            existing.DeliveryMethodId = dto.DeliveryMethodId;
            existing.CardNumber = dto.CardNumber;
            existing.CardHolder = dto.CardHolder;
            existing.ExpiryDate = dto.ExpiryDate;
            existing.CVV = dto.CVV;

            // Supprimer anciens produits
            _context.OrderProducts.RemoveRange(existing.OrderProducts);

            decimal total = 0m;

            // Ajouter nouveaux
            foreach (var p in dto.OrderProducts)
            {
                var newOp = new OrderProduct
                {
                    OrderId = id,
                    ProductId = p.ProductId,
                    Quantity = p.Quantity,
                    UnitPrice = p.UnitPrice,
                    Color = p.Color,
                    ImageUrl = p.ImageUrl
                };

                total += newOp.Quantity * newOp.UnitPrice;

                _context.OrderProducts.Add(newOp);
            }

            existing.TotalAmount = total;

            await _context.SaveChangesAsync();

            return ToDto(existing);
        }

        // -------------------------------
        // DELETE
        // -------------------------------
        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Orders.FindAsync(id);
            if (existing == null) return false;

            _context.Orders.Remove(existing);
            await _context.SaveChangesAsync();

            return true;
        }

        // -------------------------------
        // STATS
        // -------------------------------
        public async Task<decimal> GetTotalEarningsAsync()
        {
            return await _context.Orders.SumAsync(o => o.CashAmount ?? 0);
        }

        public async Task<decimal> GetTotalBalanceAsync()
        {
            return await _context.Orders.SumAsync(o =>
                (o.CashAmount ?? 0) +
                o.OrderProducts.Sum(op => op.UnitPrice * op.Quantity)
            );
        }

        public async Task<int> GetTotalProjectsAsync()
        {
            return await _context.Orders.CountAsync();
        }

        // -------------------------------
        // MAPPER
        // -------------------------------
        private static OrderDto ToDto(Order o)
        {
            return new OrderDto
            {
                Id = o.Id,
                ClientId = o.ClientId,
                OrderDate = o.OrderDate,
                PaymentMethod = o.PaymentMethod,
                CashAmount = o.CashAmount,
                PaymentDate = o.PaymentDate,
                DeliveryMethodId = o.DeliveryMethodId,
                CardNumber = o.CardNumber,
                CardHolder = o.CardHolder,
                ExpiryDate = o.ExpiryDate,
                CVV = o.CVV,
                TotalAmount = o.TotalAmount,
                InvoiceId = o.Invoice?.Id,
              
                Client = o.Client == null ? null : new ClientDto
                {
                    Id = o.Client.Id,
                    Name = o.Client.Name,
                    LastName = o.Client.LastName,
                    Email = o.Client.Email,
                    Phone = o.Client.Phone,
                    Address = o.Client.Address,
                    
                },
                OrderProducts = o.OrderProducts?.Select(op => new OrderProductDto
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                    UnitPrice = op.UnitPrice,
                    Color = op.Color,
                    ImageUrl = op.ImageUrl,
                    ProductName = op.Product?.Name
                }).ToList()
            };
        }
    }
}