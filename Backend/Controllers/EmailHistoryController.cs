using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models.dto;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/email-logs")]
    public class AdminEmailHistoryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminEmailHistoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmailHistory()
        {
         
            var logs = await _context.EmailLogs
                .OrderByDescending(e => e.SentAt)
                .ToListAsync();
            var result = logs
                .GroupBy(e => e.SentAt.Date)
                .Select(g => new EmailHistoryGroupDto
                {
                    Date = g.Key,
                    Emails = g.Select(e => new EmailHistoryItemDto
                    {
                        ToEmail = e.ToEmail,
                        Subject = e.Subject,
                        SentAt = e.SentAt
                    }).ToList()
                })
                .ToList();

            return Ok(result);
        }
    }
}