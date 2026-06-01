using System.ComponentModel.DataAnnotations;

namespace AXIOCRM.Application.DTOs
{
    public class NotificationDto
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
