namespace ProductManager.API.Models.dto
{
    public class EmailHistoryGroupDto
    {
        public DateTime Date { get; set; }
        public List<EmailHistoryItemDto> Emails { get; set; } = new();
    }

    public class EmailHistoryItemDto
    {
        public string ToEmail { get; set; } = "";
        public string Subject { get; set; } = "";
        public DateTime SentAt { get; set; }
    }
}
