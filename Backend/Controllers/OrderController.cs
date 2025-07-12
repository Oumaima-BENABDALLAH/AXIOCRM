using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IClientService _clientService;


        public OrderController(IOrderService orderService, IClientService clientService)
        {
            _orderService = orderService;
            _clientService = clientService;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Tu retournes les OrderDto correctement remplis
            var orders = await _orderService.GetAllAsync();
            var orderDtos = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                ClientId = o.ClientId,
                OrderDate = o.OrderDate,
                PaymentMethod = o.PaymentMethod,
                Products = o.OrderProducts.Select(op => new OrderProductDto
                {
                    ProductId = op.ProductId,
                    Quantity = op.Quantity,
                    ProductName = op.Product != null ? op.Product.Name : ""
                }).ToList()
            });

            return Ok(orderDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto dto)
        {
            var client = await _clientService.GetByIdAsync(dto.ClientId);
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
                    Quantity = item.Quantity,
                    UnitPrice = 0
                });
            }

           await _orderService.CreateAsync(order);
           

            return Ok(order);
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            var totalEarnings = await _orderService.GetTotalEarningsAsync();
            var totalBalance = await _orderService.GetTotalBalanceAsync();
            var totalProjects = await _orderService.GetTotalProjectsAsync();

            return Ok(new
            {
                totalEarnings,
                totalBalance,
                totalProjects
            });
        }
    }
}
