using AXIOCRM.Application.Interfaces;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.EventScheduler.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeleteEventCommandHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var ev = await _context.ScheduleEvents.FindAsync(new object[] { request.Id }, cancellationToken);
            if (ev == null) return false;

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) throw new UnauthorizedAccessException();

            var user = await _userManager.FindByIdAsync(currentUserId);
            var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin && ev.ResourceId != currentUserId)
                throw new UnauthorizedAccessException("Suppression non autorisée.");

            _context.ScheduleEvents.Remove(ev);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
