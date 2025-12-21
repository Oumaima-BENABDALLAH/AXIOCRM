namespace ProductManager.API.Models.Email
{
    public class EmailLog
    {
        public int Id { get; set; }
        public string ToEmail { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime SentAt { get; set; }
        public int ScheduleEventId { get; set; }
    }
}
