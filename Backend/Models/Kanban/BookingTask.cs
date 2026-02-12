using ProductManager.API.Models.AuthentificationJWT;

namespace ProductManager.API.Models.Kanban
{
    public class BookingTask
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public BookingTaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public string CommercialId { get; set; } = string.Empty;
        public ApplicationUser Commercial { get; set; } = null!;

        public int? ClientId { get; set; }
        public Client? Client { get; set; }

        public int? ScheduleEventId { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }      
        public DateTime? CompletedAt { get; set; }

        public bool IsArchived { get; set; }
    }
    public enum BookingTaskStatus
    {
        Pending,
        Accepted,
        InProgress,
        WaitingClient,
        Done,
        Cancelled
    }
    public enum TaskPriority
    {
        Low = 0,
        Medium = 1,
        High = 2
    }
}
