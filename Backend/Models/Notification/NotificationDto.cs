using System.ComponentModel.DataAnnotations;

namespace ProductManager.API.Models.Notification
{
    public class NotificationDto
    {

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
