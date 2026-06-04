using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Domain.Entities.Scheduler;
using AXIOCRM.Domain.Entities; 
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Identity; 
using System.Security.Claims;

namespace AXIOCRM.Application.EventScheduler.Commands.CreateEvent
{
    public class CreateEventCommandHandler
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateEventCommandHandler(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<ScheduleEventDto> HandleAsync(CreateEventCommand command, CancellationToken cancellationToken)
        {
          
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException();

          
            var user = await _userManager.FindByIdAsync(currentUserId);
            var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

            var entity = new ScheduleEvent
            {
                Title = command.Title,
                Start = command.Start,
                End = command.End,
                ResourceId = isAdmin ? command.ResourceId : currentUserId
            };

            _context.ScheduleEvents.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity.ToDto();
        }
    }
}