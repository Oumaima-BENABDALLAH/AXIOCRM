using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Clients.Queries
{
    public class GetClientsQueryHandler
    {
        private readonly AppDbContext _context;

        public GetClientsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ClientDto>> HandleAsync()
        {

            try
            {
                var client = await _context.Clients
                    .AsNoTracking()
                    .ToListAsync();

                return client.Select(o => o.ToDto()).ToList();

            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la récupération des clients.", ex);
            }

        }
    }
}
