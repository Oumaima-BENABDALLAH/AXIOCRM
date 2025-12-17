using ProductManager.API.Models.dto;
using ProductManager.API.Models.Scheduler;

namespace ProductManager.API.Models.Mappings
{
    public static class EventMapping
    {
     
            public static ScheduleEventDto ToDto(this ScheduleEvent e)
            {
                return new ScheduleEventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Start = e.Start,
                    End = e.End,
                    Description = e.Description,
                    Color = e.Color,
                    ResourceId = e.ResourceId
                };
            }

            public static ScheduleEvent ToEntity(this ScheduleEventDto dto)
            {
                return new ScheduleEvent
                {
                    Id = dto.Id,
                    Title = dto.Title,
                    Start = dto.Start,
                    End = dto.End,
                    Description = dto.Description,
                    Color = dto.Color,
                    ResourceId = dto.ResourceId
                };
            }
        
}
}
