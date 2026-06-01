namespace AXIOCRM.Application.Interfaces
{
    public interface IEventReminderJob
    {
        Task CheckTodayEvents();
    }
}
