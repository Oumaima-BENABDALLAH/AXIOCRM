using AXIOCRM.Application.DTOs;

namespace AXIOCRM.Application.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUser(string userId, NotificationDto notification);

    }
}
