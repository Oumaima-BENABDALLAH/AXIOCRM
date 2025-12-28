namespace ProductManager.API.Models.dto
{
    public class ScheduleEventDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string Color { get; set; } = "";
        public string? Description { get; set; }

        public string? ResourceId { get; set; } // UserId
        public bool ReminderSent { get; set; }
        public DateTime? LastAdminNotifiedAt { get; set; }
    }
}
