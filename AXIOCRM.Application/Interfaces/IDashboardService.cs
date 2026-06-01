using AXIOCRM.Application.DTOs;
using AXIOCRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<IEnumerable<ClientFinancialSummary>> GetFinancialSummaryAsync();

        Task<IEnumerable<InactiveClient>> GetInactiveClientsAsync(decimal seuilMontant);

        Task<IEnumerable<ClientInvoiceRank>> GetClientInvoiceRankAsync(int  IdClient);
    }
}
