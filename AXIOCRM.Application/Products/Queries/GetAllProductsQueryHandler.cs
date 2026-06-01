
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

namespace AXIOCRM.Application.Products.Queries
{
    public class GetAllProductsQueryHandler
    {
        private readonly AppDbContext _context;

        public GetAllProductsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDTO>> HandleAsync()
        {
          
            try
            {
                var products = await _context.Products
                    .AsNoTracking()
                    .Include(o => o.OrderProducts)
                    .OrderByDescending(o => o.Name)
                    .ToListAsync();

                return  products.Select(o => o.ToDto()).ToList();

              
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des produits.", ex);
            }

        }


    }
}
