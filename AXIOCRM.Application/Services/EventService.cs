using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Domain.Entities.Scheduler;
using AXIOCRM.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace AXIOCRM.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventService _eventService;
        private readonly IIdentityService _identityService;

        public EventService(IEventService eventService , IIdentityService identityService)
        {
            _eventService = eventService;
           _identityService = identityService;
        }

        public async Task<List<ScheduleEventDto>> GetAllAsync()
        {
            // On récupère tout, puis on mappe en DTO
            var events = await _eventService.GetAllAsync();
           return events.Select(e => e.ToDto()).ToList();
        }

        public async Task<ScheduleEventDto?> GetByIdAsync(int id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev == null) return null;

            if (!await IsAuthorized(ev.ResourceId))
                throw new UnauthorizedAccessException("Accès refusé.");

            return ev.ToDto();
        }

      

      

        public async Task<bool> DeleteAsync(int id)
        {
            var ev = await _eventService.GetByIdAsync(id);
            if (ev == null) return false;

            if (!await IsAuthorized(ev.ResourceId))
                throw new UnauthorizedAccessException();

            await _eventService.DeleteAsync(id);
            return true;
        }

  
        private async Task<bool> IsAuthorized(string resourceId)
        {
            var userId = await _identityService.GetCurrentUserIdAsync();
            if (userId == null) return false;

            var isAdmin = await _identityService.IsInRoleAsync(userId, "Admin");
            return isAdmin || resourceId == userId;
        }


    }
}
