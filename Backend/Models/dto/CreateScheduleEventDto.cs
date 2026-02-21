namespace ProductManager.API.Models.dto
{
    public class CreateScheduleEventDto
    {
        public string Title { get; set; } = "";
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public string? Description { get; set; }
        public string? Color { get; set; }

        public string ResourceId { get; set; } = ""; 
    }
}