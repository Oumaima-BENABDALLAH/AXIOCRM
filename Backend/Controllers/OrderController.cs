using Google;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Hubs;
using ProductManager.API.Models;
using ProductManager.API.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            return order == null ? NotFound() : Ok(order);
        }

        // CREATE
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] OrderDto dto)
        {
            var created = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // UPDATE
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> Update(int id, [FromBody] OrderDto dto)
        {
            var updated = await _orderService.UpdateAsync(id, dto);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removed = await _orderService.DeleteAsync(id);
            return removed ? NoContent() : NotFound();
        }
    }

}
