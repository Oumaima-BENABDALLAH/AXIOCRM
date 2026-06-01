using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Application.Orders.Commands.CreateOrder;
using AXIOCRM.Application.Orders.Commands.DeleteOrder;
using AXIOCRM.Application.Orders.Commands.UpdateOrder;
using AXIOCRM.Application.Orders.Queries;
using Google;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
      
        private readonly CreateOrderCommandHandler _handler;
        private readonly GetOrdersQueryHandler _getOrdersHandler;
        private readonly GetOrderByIdQueryHandler _getOrderByIdHandler;
        private readonly DeleteOrderCommandHandler _deleteOrderCommandHandler;
        private readonly UpdateOrderCommandHandler _updateOrderCommandHandler;
        public OrderController(CreateOrderCommandHandler handler ,
                                GetOrdersQueryHandler getOrdersHandler,
                                GetOrderByIdQueryHandler getOrderByIdHandler,
                                DeleteOrderCommandHandler deleteOrderCommandHandler,
                                UpdateOrderCommandHandler updateOrderCommandHandler)
        {
            _handler = handler;
            _getOrdersHandler = getOrdersHandler;
            _getOrderByIdHandler = getOrderByIdHandler;
            _deleteOrderCommandHandler = deleteOrderCommandHandler;
            _updateOrderCommandHandler = updateOrderCommandHandler;
         
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await _getOrdersHandler.HandleAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            var order = await _getOrderByIdHandler.HandleAsync(id);
            return order == null ? NotFound() : Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderCommand command)
        {
            var created = await _handler.HandleAsync(command);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> Update(int id, [FromBody] UpdateOrderCommand command)
        {
            var updated = await _updateOrderCommandHandler.HandleAsync(command);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var removed =  await _deleteOrderCommandHandler.HandleAsync(id);
            return removed ? NoContent() : NotFound();
        }
    }

}
