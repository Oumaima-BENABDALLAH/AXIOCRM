using ProductManager.API.Models.AuthentificationJWT;

namespace ProductManager.API.Models.Scheduler
{
    public class ScheduleEvent
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string Color { get; set; } = "#3788d8";   // FullCalendar-ready
        public string? Description { get; set; }

   
        public string? ResourceId { get; set; }   // UserId
                                                  //  public Resource? Resource { get; set; }
        public ApplicationUser? Resource { get; set; }
        public bool AdminNotified { get; set; }      // SignalR
        public DateTime? LastAdminNotifiedAt { get; set; }
        public bool EmailSent { get; set; }         // Email commercial
    }
}
