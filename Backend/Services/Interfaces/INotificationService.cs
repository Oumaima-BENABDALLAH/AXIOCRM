using ProductManager.API.Models.Notification;
using ProductManager.API.Models.Scheduler;

namespace ProductManager.API.Services.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUser(string userId, NotificationDto notification);
    }
}
