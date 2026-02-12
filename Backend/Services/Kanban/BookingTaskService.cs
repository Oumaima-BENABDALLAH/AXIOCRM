using Microsoft.EntityFrameworkCore;
using ProductManager.API.Data;
using ProductManager.API.Models.dto;
using ProductManager.API.Models.Kanban;
using ProductManager.API.Models.Notification;
using ProductManager.API.Services.Interfaces;

namespace ProductManager.API.Services.Kanban
{
    public class BookingTaskService : IBookingTaskService
    {
        private readonly AppDbContext _context;
        private readonly IEventService _eventService;
        private readonly INotificationService _notificationService;

        public BookingTaskService(
            AppDbContext context,
            IEventService eventService,
            INotificationService notificationService)
        {
            _context = context;
            _eventService = eventService;
            _notificationService = notificationService;
        }

        public async Task<List<BookingTaskDto>> GetKanbanAsync(string userId, bool isAdmin)
        {
            var query = _context.BookingTasks
                .Include(t => t.Client)
                .Include(t => t.Commercial)
                .AsQueryable();

            if (!isAdmin)
                query = query.Where(t => t.CommercialId == userId);

            return await query
                .OrderBy(t => t.CreatedAt)
                .Select(t => new BookingTaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    ClientId = t.ClientId,
                    ClientName = t.Client != null ? t.Client.Name : null,
                    CommercialId = t.CommercialId,
                    CommercialName = t.Commercial.FullName ?? t.Commercial.Email!,
                    ScheduleEventId = t.ScheduleEventId,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate,
                    Priority = t.Priority

                })
                .ToListAsync();
        }

        public async Task<BookingTaskDto> CreateAsync(CreateBookingTaskDto dto, string userId)
        {
            var task = new BookingTask
            {
                Title = dto.Title,
                Description = dto.Description,
                ClientId = dto.ClientId,
                DueDate = dto.DueDate,
                CommercialId = dto.CommercialId,   
                Status = dto.Status,               
                Priority = dto.Priority            
            };

            _context.BookingTasks.Add(task);
            await _context.SaveChangesAsync();

            var commercial = await _context.Users.FindAsync(task.CommercialId);
            var client = await _context.Clients.FindAsync(task.ClientId);

            return new BookingTaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                ClientId = task.ClientId,
                ClientName = client?.Name,
                CommercialId = task.CommercialId,
                CommercialName = commercial?.FullName ?? commercial?.Email,
                CreatedAt = task.CreatedAt,
                DueDate = task.DueDate,
                Priority = task.Priority
            };
        }



        public async Task<bool> UpdateStatusAsync(
            int taskId,
            BookingTaskStatus newStatus,
            string userId,
            bool isAdmin)
        {
            var task = await _context.BookingTasks.FindAsync(taskId);
            if (task == null) return false;

            if (!isAdmin && task.CommercialId != userId)
                throw new UnauthorizedAccessException();

            task.Status = newStatus;

            await _context.SaveChangesAsync();
            return true;
        }

        

        public async Task<ScheduleEventDto> PlanifyAsync(
            int taskId,
            PlanifyTaskDto dto,
            string userId,
            bool isAdmin)
        {
            var task = await _context.BookingTasks
                .Include(t => t.Commercial)
                .FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null)
                throw new Exception("Task not found");

            if (!isAdmin && task.CommercialId != userId)
                throw new UnauthorizedAccessException();

            if (task.ScheduleEventId != null)
                throw new Exception("Task already planned");

            var ev = await _eventService.CreateAsync(new ScheduleEventDto
            {
                Title = task.Title,
                Start = dto.Start,
                End = dto.End,
                Color = dto.Color,
                ResourceId = task.CommercialId,
                Description = $"Planned from Kanban task #{task.Id}"
            }, userId, isAdmin);

            task.ScheduleEventId = ev.Id;
            task.Status = BookingTaskStatus.InProgress;

            await _context.SaveChangesAsync();

            await _notificationService.NotifyUser(
                task.CommercialId,
                new NotificationDto
                {
                    Title = "Task planned",
                    Message = $"Task '{task.Title}' has been scheduled"
                });

            return ev;
        }
        public async Task<bool> DeleteAsync(int taskId, string userId, bool isAdmin)
        {
            var task = await _context.BookingTasks.FindAsync(taskId);

            if (task == null)
                return false;

            if (!isAdmin && task.CommercialId != userId)
                throw new UnauthorizedAccessException();

            _context.BookingTasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateAsync(int taskId, UpdateBookingTaskDto dto, string userId,bool isAdmin)
        {
            var task = await _context.BookingTasks.FindAsync(taskId);
            if (task == null)
                return false;

            if (!isAdmin && task.CommercialId != userId)
                throw new UnauthorizedAccessException();

            task.Title = dto.Title;
            task.Description = dto.Description;
            task.ClientId = dto.ClientId;
            task.DueDate = dto.DueDate;
            task.Status = dto.Status;
            task.Priority = dto.Priority;

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
