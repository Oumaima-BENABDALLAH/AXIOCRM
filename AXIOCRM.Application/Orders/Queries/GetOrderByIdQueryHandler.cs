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
    public  class GetOrderByIdQueryHandler
    {
        private readonly AppDbContext _context;
        public GetOrderByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrderDto?> HandleAsync(int id)
        {


            try
            {
                var order = await _context.Orders
                    .AsNoTracking()
                    .Include(o => o.Client)
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .FirstOrDefaultAsync(o => o.Id == id);
                return order?.ToDto();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de la commande {id}.", ex);
            }
        }
    }
}
