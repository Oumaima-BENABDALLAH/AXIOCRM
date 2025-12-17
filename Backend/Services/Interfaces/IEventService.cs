using ProductManager.API.Models.dto;
using ProductManager.API.Models.Scheduler;

namespace ProductManager.API.Services.Interfaces
{
    public interface IEventService
    {
        Task<List<ScheduleEventDto>> GetAllAsync(string currentUserId, bool isAdmin);
        Task<ScheduleEventDto?> GetByIdAsync(int id, string currentUserId, bool isAdmin);
        Task<ScheduleEventDto> CreateAsync(CreateScheduleEventDto dto, string currentUserId, bool isAdmin);
        Task<bool> UpdateAsync(int id, CreateScheduleEventDto dto, string currentUserId, bool isAdmin);
        Task<bool> DeleteAsync(int id, string currentUserId, bool isAdmin);

        Task<List<SchedulerResourceDto>> GetCommercialsAsync();
    }
}
