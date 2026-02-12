using ProductManager.API.Models.Kanban;

namespace ProductManager.API.Models.dto
{
    public class BookingTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public BookingTaskStatus Status { get; set; }

        public int? ClientId { get; set; }
        public string? ClientName { get; set; }

        public string CommercialId { get; set; } = string.Empty;
        public string CommercialName { get; set; } = string.Empty;

        public int? ScheduleEventId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskPriority Priority { get; set; }

    }

}
