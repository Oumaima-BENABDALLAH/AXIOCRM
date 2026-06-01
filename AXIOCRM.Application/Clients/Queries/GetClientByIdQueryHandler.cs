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
    public  class GetClientByIdQueryHandler
    {
        private readonly AppDbContext _context;
        public GetClientByIdQueryHandler(AppDbContext context)
            {

            _context = context;
        }

        public async Task<ClientDto?> HandleAsync(int id)
        {


            try
            {
                var client = await _context.Clients
                    .AsNoTracking()
                    .FirstOrDefaultAsync(o => o.Id == id);
                return client?.ToDto();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erreur lors de la récupération de client {id}.", ex);
            }
        }
    }
}
