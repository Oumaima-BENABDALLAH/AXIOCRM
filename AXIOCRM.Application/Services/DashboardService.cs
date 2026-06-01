using AXIOCRM.Application.Interfaces;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Services
{
    public class DashboardService : IDashboardService
    {
        
        private readonly AppDbContext _context;
        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<IEnumerable<ClientInvoiceRank>> GetClientInvoiceRankAsync(int IdClient)
        {
            var paramClientId = new SqlParameter("@ClientId", IdClient);
            return await _context.ClientInvoiceRanks.FromSqlRaw("EXEC dbo.sp_GetClientInvoicesWithRank  @ClientId" , paramClientId)
                  .ToListAsync();
        }          

        public async  Task<IEnumerable<ClientFinancialSummary>> GetFinancialSummaryAsync()
         {
            return await _context.ClientFinancialSummaries.ToListAsync();
           }

        public async Task<IEnumerable<InactiveClient>> GetInactiveClientsAsync(decimal seuilMontant)
        {
            var paramSeuil = new SqlParameter("@SeuilMontant", seuilMontant);
            return await _context.InactiveClients.FromSqlRaw("EXEC dbo.sp_GetInactiveClients @SeuilMontant", paramSeuil)
                .ToListAsync();
        }
    }
}
