namespace ProductManager.API.Services.Interfaces
{
    public interface ICommercialEmailReminderJob
    {
        Task SendEmailReminders();
    }
}
