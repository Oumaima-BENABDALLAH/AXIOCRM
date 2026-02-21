using ProductManager.API.Data;
using ProductManager.API.Models.dto;
using ProductManager.API.Services.Interfaces;
using ProductManager.API.Models.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ProductManager.API.Models.AuthentificationJWT;
using ProductManager.API.Models.Scheduler;

namespace ProductManager.API.Services
{
    public class EventService : IEventService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventService(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

  
        public async Task<List<ScheduleEventDto>> GetAllAsync(string userId, bool isAdmin)
        {
            var query = _context.ScheduleEvents.AsQueryable();

            return await query.Select(e => e.ToDto()).ToListAsync();
        }

        public async Task<ScheduleEventDto> CreateAsync(
            ScheduleEventDto dto,
            string userId,
            bool isAdmin)
        {
            bool isOwnResource = string.Equals(dto.ResourceId, userId, StringComparison.OrdinalIgnoreCase);

            if (!isAdmin && !string.IsNullOrEmpty(dto.ResourceId) && !isOwnResource)
            {
                throw new UnauthorizedAccessException("Accès refusé : Identifiants non correspondants.");
            }

            var entity = new ScheduleEvent
            {
                Title = dto.Title,
                Start = dto.Start,
                End = dto.End,
                Description = dto.Description,
                Color = dto.Color ?? "#3788d8",
                ResourceId = isAdmin ? dto.ResourceId : userId
            };

            _context.ScheduleEvents.Add(entity);
            await _context.SaveChangesAsync();

            return entity.ToDto();
        }
        public async Task<bool> UpdateAsync(
          int id,
          ScheduleEventDto dto,
          string userId,
          bool isAdmin)
           {
            var ev = await _context.ScheduleEvents.FindAsync(id);
            if (ev == null) return false;

            if (!isAdmin && ev.ResourceId != userId)
                throw new UnauthorizedAccessException();

            ev.Title = dto.Title;
            ev.Start = dto.Start;
            ev.End = dto.End;
            ev.Description = dto.Description;
            ev.Color = dto.Color ?? ev.Color;

            
            if (!string.IsNullOrWhiteSpace(dto.ResourceId))
            {
                ev.ResourceId = dto.ResourceId;
            }
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<bool> DeleteAsync(int id, string userId, bool isAdmin)
        {
            var ev = await _context.ScheduleEvents.FindAsync(id);
            if (ev == null) return false;

            if (!isAdmin && ev.ResourceId != userId)
                throw new UnauthorizedAccessException();

            _context.ScheduleEvents.Remove(ev);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<SchedulerResourceDto>> GetCommercialsAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<SchedulerResourceDto>();

            foreach (var user in users)
            {
                if (await _userManager.IsInRoleAsync(user, "Commercial"))
                {
                    result.Add(new SchedulerResourceDto
                    {
                        Id = user.Id,
                        Name = user.FullName ?? user.Email!
                    });
                }
            }
            return result;
        }

        public async Task<ScheduleEventDto?> GetByIdAsync(
      int id,
      string currentUserId,
      bool isAdmin)
        {
            var ev = await _context.ScheduleEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return null;
            if (!isAdmin && ev.ResourceId != currentUserId)
                return null;

            return ev.ToDto();
        }
    }
}
