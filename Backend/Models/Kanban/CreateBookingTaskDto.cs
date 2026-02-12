namespace ProductManager.API.Models.Kanban
{
    public class CreateBookingTaskDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        public int? ClientId { get; set; }
        public DateTime? DueDate { get; set; }

        public string? CommercialId { get; set; }
        public BookingTaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
    }

}
