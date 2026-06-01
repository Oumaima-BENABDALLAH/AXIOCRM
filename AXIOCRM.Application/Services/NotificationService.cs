

using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ProductManager.API.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyUser(string userId, NotificationDto notification)
        {
            await _hub.Clients
                .User(userId)
                .SendAsync("ReceiveNotification", notification);
        }
    }

}