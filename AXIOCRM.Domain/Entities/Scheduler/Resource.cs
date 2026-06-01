namespace AXIOCRM.Domain.Entities.Scheduler
{
    public class Resource
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<ScheduleEvent> Events { get; set; } = new List<ScheduleEvent>();
    }
}
