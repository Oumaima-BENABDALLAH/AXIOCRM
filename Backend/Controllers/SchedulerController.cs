using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ProductManager.API.Models.dto;
using ProductManager.API.Services.Interfaces;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ev = await _service.GetByIdAsync(id, UserId, IsAdmin);
            return ev == null ? NotFound() : Ok(ev);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateScheduleEventDto dto)
        {
            var created = await _service.CreateAsync(dto, UserId, IsAdmin);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateScheduleEventDto dto)
        {
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