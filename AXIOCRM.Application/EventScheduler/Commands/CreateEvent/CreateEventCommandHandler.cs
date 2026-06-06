using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Domain.Entities; 
using AXIOCRM.Domain.Entities.Scheduler;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Identity; 
using System.Security.Claims;

namespace AXIOCRM.Application.EventScheduler.Commands.CreateEvent
{
    public class CreateEventCommandHandler
    {
        private readonly AppDbContext _context;
        private readonly IIdentityService _identityService;

        public CreateEventCommandHandler(AppDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<ScheduleEventDto> HandleAsync(CreateEventCommand command, CancellationToken cancellationToken)
        {

            var currentUserId = await _identityService.GetCurrentUserIdAsync()
                                 ?? throw new UnauthorizedAccessException();

          
            var isAdmin = await _identityService.IsInRoleAsync(currentUserId, "Admin");

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