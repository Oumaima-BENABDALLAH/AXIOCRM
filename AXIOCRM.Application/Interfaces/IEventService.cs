using AXIOCRM.Application.DTOs;

namespace AXIOCRM.Application.Interfaces
{
    public interface IEventService
    {
        Task<List<ScheduleEventDto>> GetAllAsync();
        Task<ScheduleEventDto?> GetByIdAsync(int id);
        Task<ScheduleEventDto> CreateAsync(ScheduleEventDto dto);
        Task<bool> UpdateAsync(int id, ScheduleEventDto dto);
        Task<bool> DeleteAsync(int id);

    }
}
