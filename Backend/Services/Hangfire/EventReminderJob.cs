using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models.AuthentificationJWT;
using ProductManager.API.Services.Interfaces;
using ProductManager.API.Models.Notification;
using Google;
using Hangfire;

namespace ProductManager.API.Services.Hangfire
{
    public class EventReminderJob : IEventReminderJob
    {
        private readonly AppDbContext _context;
        private readonly INotificationService _notificationService;
        public EventReminderJob(AppDbContext context, INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        public async Task CheckTodayEvents()
        {
            Console.WriteLine("EventReminderJob STARTED");

            var today = DateTime.Today;

            var events = await _context.ScheduleEvents
                .Include(e => e.Resource)
                  .Where(e => e.Start.Date == today)
                  .ToListAsync();

            if (!events.Any())
                return;

            var admins = await _context.Users
                .Where(u => u.Role == "Admin")
                .ToListAsync();

            foreach (var ev in events)
            {
                var notification = new NotificationDto
                {
                    Title = "Event reminder",
                    Message =
                        $"The commercial {ev.Resource?.FullName} will have " +
                        $"{ev.Title} from {ev.Start:HH:mm} to {ev.End:HH:mm}"
                };

                foreach (var admin in admins)
                {
                    await _notificationService.NotifyUser(admin.Id, notification);
                }

                
            }

            await _context.SaveChangesAsync();
        }
    }
  }
