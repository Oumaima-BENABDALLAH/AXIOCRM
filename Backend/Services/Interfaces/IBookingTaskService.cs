using ProductManager.API.Models.dto;
using ProductManager.API.Models.Kanban;

namespace ProductManager.API.Services.Interfaces
{
    public interface IBookingTaskService
    {
        Task<List<BookingTaskDto>> GetKanbanAsync(string userId, bool isAdmin);
        Task<BookingTaskDto> CreateAsync(CreateBookingTaskDto dto, string userId);
        Task<bool> DeleteAsync(int taskId, string userId, bool isAdmin);
        Task<bool> UpdateStatusAsync(int taskId,BookingTaskStatus newStatus,string userId,bool isAdmin);
        Task<bool> UpdateAsync(int taskId, UpdateBookingTaskDto dto,string userId,bool isAdmin);
        Task<ScheduleEventDto> PlanifyAsync(int taskId,PlanifyTaskDto dto,string userId,bool isAdmin);
    }

}
