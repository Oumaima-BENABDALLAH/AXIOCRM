using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using AXIOCRM.Application.DTOs;
using AXIOCRM.Infrastructure.Persistence;
using AXIOCRM.Application.Interfaces;

namespace AXIOCRM.Application.Services
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
