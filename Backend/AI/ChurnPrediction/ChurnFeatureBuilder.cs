using System;
using System.Linq;
using Google;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;

namespace ProductManager.API.AI.ChurnPrediction
{
    public class ChurnFeatureBuilder
    {
        private readonly AppDbContext _context;

        public ChurnFeatureBuilder(AppDbContext context)
        {
            _context = context;
        }

        public ChurnInputModel Build(int clientId) 
        {
            var orders = _context.Orders
                    .Where(o => o.ClientId == clientId)
                    .Include(o => o.Invoice)
                    .ToList();

            var lastOrder = orders
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefault();

            return new ChurnInputModel
            {
                TotalOrders = orders.Count(), 
                TotalSpent = (float)orders.Sum(o => o.TotalAmount),
                DaysSinceLastOrder = lastOrder == null
                    ? 999
                    : (float)(DateTime.UtcNow - lastOrder.OrderDate).TotalDays,
                LatePayments = orders.Count(o =>o.Invoice != null &&
                                            o.PaymentDate.HasValue &&
                                            o.PaymentDate.Value > o.Invoice.DueDate)
            };
        }
    }
}
