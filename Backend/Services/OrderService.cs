using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Hubs;
using ProductManager.API.Models;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Services
{
    public class OrderService :IOrderService
    {
        private readonly AppDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;
        public OrderService(AppDbContext context , IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
            => await _context.Orders.Include(c => c.OrderProducts).ThenInclude(op => op.Product)
                                    .Include(c => c.Client)
                                    .OrderByDescending(c => c.OrderDate)
            .ToListAsync();

        public async Task<Order> GetByIdAsync(int id)
            => await _context.Orders
            .Include(c => c.OrderProducts).ThenInclude(op => op.Product).FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await AddOrder(order);
            return order;
        }

        public async Task<Order> UpdateAsync(int id, Order updated)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return null;

            order.Client = updated.Client;
            order.OrderDate = updated.OrderDate;
            order.PaymentMethod = updated.PaymentMethod;
            order.TotalAmount = updated.TotalAmount;

            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task <decimal> GetTotalEarningsAsync()
        {
            return await _context.Orders.SumAsync(o => o.OrderProducts.Sum(p => p.Quantity * p.UnitPrice));
        }
        public async Task<decimal> GetTotalBalanceAsync()
        {
            return await _context.Orders.SumAsync(o => o.OrderProducts.Sum(p => p.Quantity * p.UnitPrice)/100);
        }

        public async Task<int> GetTotalProjectsAsync()
        {
            return await _context.Orders.CountAsync();
        }


        // Notifier les clients connectés via SignalR

        public async Task AddOrder(Order order)
        {
            // Envoie un message à tous les clients connectés au hub
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", $"📁 pRODUCT ajouté: {order.OrderDate}");
        }

    }
}
