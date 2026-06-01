using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.EventScheduler.Commands.CreateEvent;
using AXIOCRM.Application.EventScheduler.Commands.DeleteEvent;
using AXIOCRM.Application.EventScheduler.Commands.UpdateEvent;
using AXIOCRM.Application.EventScheduler.Queries.GetAllEvents;
using AXIOCRM.Application.EventScheduler.Queries.GetEventsById;
using AXIOCRM.Application.Interfaces;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProductManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/scheduler")]
    public class SchedulerController : ControllerBase
    {

        private readonly IMediator _mediator;
        public SchedulerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        private string UserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        private bool IsAdmin =>
            User.IsInRole("Admin");

        [HttpGet]
        public async Task<ActionResult<List<ScheduleEventDto>>> GetAll()
            => await _mediator.Send(new GetAllEventsQuery());

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduleEventDto>> GetById(int id)
        {
            var result = await _mediator.Send(new GetEventByIdQuery(id));
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEventCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEventCommand command)
        {
            if (id != command.Id)
                return BadRequest("ID mismatch");

            var ok = await _mediator.Send(command);
            return ok ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _mediator.Send(new DeleteEventCommand(id));
            return ok ? Ok() : NotFound();
        }

    }
}