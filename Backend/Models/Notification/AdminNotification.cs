namespace ProductManager.API.Models.Notification
{
    public class AdminNotification
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";
        public string Message { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsSentToAdmin { get; set; } = false;
    }
}
