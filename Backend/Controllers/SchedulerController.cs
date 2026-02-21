using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProductManager.API.Models.dto;
using ProductManager.API.Services.Interfaces;
using Hangfire;

namespace ProductManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/scheduler")]
    public class SchedulerController : ControllerBase
    {
        private readonly IEventService _service;

        public SchedulerController(IEventService service)
        {
            _service = service;
        }

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        private bool IsAdmin =>
            User.IsInRole("Admin");

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync(UserId, IsAdmin));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ScheduleEventDto dto)
        {
            try
            {
                var created = await _service.CreateAsync(dto, UserId, IsAdmin);
                return Ok(created);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); 
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ScheduleEventDto dto)
        {
            Console.WriteLine(" UPDATE API HIT ");
            Console.WriteLine($"URL ID: {id}");
            Console.WriteLine($"DTO ID: {dto.Id}");
            Console.WriteLine($"Title: {dto.Title}");
            Console.WriteLine($"Start: {dto.Start}");
            Console.WriteLine($"End: {dto.End}");
            Console.WriteLine($"Color: {dto.Color}");
            Console.WriteLine($"ResourceId: {dto.ResourceId}");
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var ok = await _service.UpdateAsync(id, dto, UserId, IsAdmin);
            return ok ? Ok() : NotFound();
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id, UserId, IsAdmin);
            return ok ? Ok() : NotFound();
        }

        [HttpGet("resources/commercials")]
        public async Task<IActionResult> GetCommercials()
        {
            return Ok(await _service.GetCommercialsAsync());
        }
      
    }
}