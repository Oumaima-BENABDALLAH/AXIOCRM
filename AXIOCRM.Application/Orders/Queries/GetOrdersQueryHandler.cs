using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace AXIOCRM.Application.Orders.Queries
{
    public  class GetOrdersQueryHandler
    {
        private readonly AppDbContext _context;

        public GetOrdersQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderDto>> HandleAsync()
        {
           

            try
            {
                var orders = await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.Client)
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                return orders.Select(o => o.ToDto()).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des commandes.", ex);
            }

        }


    }
}
