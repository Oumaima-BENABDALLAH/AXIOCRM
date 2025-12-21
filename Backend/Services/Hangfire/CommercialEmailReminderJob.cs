using Hangfire;
using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models.Email;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Services.Hangfire
{
    public class CommercialEmailReminderJob : ICommercialEmailReminderJob
    {
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public CommercialEmailReminderJob(
            AppDbContext context,
            IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [DisableConcurrentExecution(300)]
        public async Task SendEmailReminders()
        {
            var today = DateTime.Today;

            var events = await _context.ScheduleEvents
                .Include(e => e.Resource)
                .Where(e =>
                    e.Start.Date == today &&
                    !e.EmailSent &&
                    e.Resource != null &&
                    e.Resource.Email != null)
                .ToListAsync();

            foreach (var ev in events)
            {
                var subject = $"Rappel : {ev.Title}";
                var body = $@"
                <p>Bonjour {ev.Resource!.FullName},</p>
                <p>Vous avez un événement aujourd’hui :</p>
                <ul>
                    <li><b>{ev.Title}</b></li>
                    <li>Début : {ev.Start:HH:mm}</li>
                    <li>Fin : {ev.End:HH:mm}</li>
                </ul>
            ";

                await _emailService.SendAsync(
                    ev.Resource.Email!,
                    subject,
                    body
                );

                _context.EmailLogs.Add(new EmailLog
                {
                    ToEmail = ev.Resource.Email!,
                    Subject = subject,
                    Body = body,
                    SentAt = DateTime.Now,
                    ScheduleEventId = ev.Id
                });

                ev.EmailSent = true;
            }

            await _context.SaveChangesAsync();
        }
    }
}
