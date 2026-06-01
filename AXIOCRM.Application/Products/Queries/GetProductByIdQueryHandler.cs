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

namespace AXIOCRM.Application.Products.Queries
{
    public class GetProductByIdQueryHandler
    {
        private readonly AppDbContext _context;
        public GetProductByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDTO?> HandleAsync(int id)
        {
         
            try
            {
                var product = await _context.Products
                              .AsNoTracking()
                              .Include(c => c.OrderProducts)
                              .FirstOrDefaultAsync(c => c.Id == id);

                if (product == null) return null;

                return product?.ToDto();
            

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving the product.", ex);

            }
        }
   }
}
