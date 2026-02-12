using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductManager.API.Models.AuthentificationJWT;
using ProductManager.API.Models.dto;
using ProductManager.API.Models.Kanban;
using ProductManager.API.Services.Interfaces;
using System.Security.Claims;

namespace ProductManager.API.Controllers
{
    [ApiController]
    [Route("api/booking-tasks")]
    [Authorize]
    public class BookingTasksController : ControllerBase
    {
        private readonly IBookingTaskService _service;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingTasksController(
            IBookingTaskService service,
            UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet("kanban")]
        [AllowAnonymous]
        public async Task<IActionResult> GetKanban()
        {

            const bool isAdmin = true;
            const string fakeUserId = "ANONYMOUS";

            var result = await _service.GetKanbanAsync(fakeUserId, isAdmin);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateBookingTaskDto dto)
        {
            const bool isAdmin = true;
            const string fakeUserId = "ANONYMOUS";

            var task = await _service.CreateAsync(dto, fakeUserId);
            return Ok(task);
        }
        [HttpPut("{id}/status")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateStatus(int id,[FromBody] BookingTaskStatus status)
        {
            const bool isAdmin = true;
            const string fakeUserId = "ANONYMOUS";

            await _service.UpdateStatusAsync(id, status, fakeUserId, isAdmin);
            return NoContent();
        }

        [HttpPost("{id}/planify")]
        [AllowAnonymous]
        public async Task<IActionResult> Planify( int id, PlanifyTaskDto dto)
        {
            const bool isAdmin = true;
            const string fakeUserId = "ANONYMOUS";

            var ev = await _service.PlanifyAsync(id, dto, fakeUserId, isAdmin);
            return Ok(ev);
        }
        [HttpDelete("{id}")]
        [AllowAnonymous] 
        public async Task<IActionResult> Delete(int id)
        {
            const bool isAdmin = true;
            const string fakeUserId = "ANONYMOUS";

            var deleted = await _service.DeleteAsync(id, fakeUserId, isAdmin);

            if (!deleted)
                return NotFound();

            return NoContent(); 
        }
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, UpdateBookingTaskDto dto)
        {
            const bool isAdmin = true;
            const string fakeUserId = "ANONYMOUS";

            var updated = await _service.UpdateAsync(id, dto, fakeUserId, isAdmin);
            if (!updated)
                return NotFound();

            return NoContent();
        }
    }
}