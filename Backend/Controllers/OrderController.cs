using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }
  
        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _context.OrderProducts.ToListAsync());
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
        {
            var client = await _context.Clients.FindAsync(dto.ClientId);
            if (client == null) return NotFound("Client not found");

            var order = new Order
            {
                ClientId = dto.ClientId,
                OrderDate = dto.OrderDate,
                PaymentMethod = dto.PaymentMethod
            };

            foreach (var item in dto.Products)
            {
                order.OrderProducts.Add(new OrderProduct
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(order);
        }
    }
}
