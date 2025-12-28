using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Hubs;
using ProductManager.API.Models;
using ProductManager.API.Models.AuthentificationJWT;
using ProductManager.API.Models.Notification;
using ProductManager.API.Models.Scheduler;
using ProductManager.API.Services.Interfaces;

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