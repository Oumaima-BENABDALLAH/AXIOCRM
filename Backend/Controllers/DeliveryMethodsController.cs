using Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models.Invoice;
using ProductManager.API.Services;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryMethodsController : ControllerBase
    {

        private readonly IDeliveryMethod _deliveryMethodService;
        public DeliveryMethodsController(IDeliveryMethod deliveryMethod)
        {

            _deliveryMethodService = deliveryMethod;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeliveryMethod>>> GetDeliveryMethods()
        {
            var methods = await _deliveryMethodService.GetAllAsync();
            return Ok(methods);
        }

        // GET: api/deliverymethods/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethod(int id)
        {
            var method = await _deliveryMethodService.GetByIdAsync(id);
            if (method == null)
                return NotFound();
            return Ok(method);
        }

        // POST: api/deliverymethods
        [HttpPost]
        public async Task<ActionResult<DeliveryMethod>> PostDeliveryMethod(DeliveryMethod method)
        {
            var created = await _deliveryMethodService.CreateAsync(method);
            return CreatedAtAction(nameof(GetDeliveryMethod), new { id = created.Id }, created);
        }

        // PUT: api/deliverymethods/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeliveryMethod(int id, DeliveryMethod method)
        {
            if (id != method.Id)
                return BadRequest();

            var updated = await _deliveryMethodService.UpdateAsync(id, method);
            if (updated == null)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/deliverymethods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryMethod(int id)
        {
            var deleted = await _deliveryMethodService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}