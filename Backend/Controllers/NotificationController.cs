using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ProductManager.API.Hubs;
using ProductManager.API.Models;

namespace ProductManager.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public NotificationController(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        [HttpGet("test")]
        public async Task<IActionResult> SendTestNotification()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", "✅ Notification test depuis le backend !");
            return Ok();
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Message))
                return BadRequest("Message is required.");

            await _hubContext.Clients.All.SendAsync("ReceiveNotification", dto.Message);
            return Ok();
        }
    }
    
}
