namespace ProductManager.API.Services.Interfaces
{
    public interface IEventReminderJob
    {
        Task CheckTodayEvents();
    }
}
