using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Hubs;
using System.Security.Claims;

[ApiController]
[Route("api/notification")]
[Authorize(Roles = "Admin")]
public class NotificationController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationController(
        AppDbContext context,
        IHubContext<NotificationHub> hub)
    {
        _context = context;
        _hub = hub;
    }

    [HttpGet("admin/push")]
    public async Task<IActionResult> PushAdminNotifications()
    {
        var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var notifications = await _context.AdminNotifications
            .Where(n => !n.IsSentToAdmin)
            .OrderBy(n => n.CreatedAt)
            .ToListAsync();

        foreach (var n in notifications)
        {
            await _hub.Clients
                .Group(adminId!)
                .SendAsync("ReceiveNotification", new
                {
                    title = n.Title,
                    message = n.Message
                });

            n.IsSentToAdmin = true;
        }

        await _context.SaveChangesAsync();

        return Ok();
    }
}
