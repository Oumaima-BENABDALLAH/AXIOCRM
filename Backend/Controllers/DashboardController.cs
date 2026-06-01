using AXIOCRM.Application.Interfaces;
using AXIOCRM.Application.Services;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ProductManager.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DashboardController : ControllerBase
    {
       
        private readonly DashboardService _dashboardService;
        public DashboardController( DashboardService dashboardService)
        {
          
            _dashboardService = dashboardService;
        }
        [HttpGet("financial-summary")]
        public async Task<IActionResult> GetFinancialSummary()
        {
            var summary = await _dashboardService.GetFinancialSummaryAsync();
            return Ok(summary);
        }


        [HttpGet("inactive-clients")]
        public async Task<IActionResult> GetInactiveClients([FromQuery] decimal seuil = 1000)
        {
            var inactiveClients = await _dashboardService.GetInactiveClientsAsync(seuil);
            return Ok(inactiveClients);
        }
         [HttpGet("top-products")]
        public async Task<IActionResult> ClientInvoiceRank([FromQuery] int  idClient)
        {
            var clientInvoiceRank = await _dashboardService.GetClientInvoiceRankAsync(idClient);
            return Ok(clientInvoiceRank);
        }

    }
}
