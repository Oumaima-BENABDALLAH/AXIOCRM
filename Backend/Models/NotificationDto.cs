using System.ComponentModel.DataAnnotations;

namespace ProductManager.API.Models
{
    public class NotificationDto
    {
        [Required]
        public string Message { get; set; }
    }
}
