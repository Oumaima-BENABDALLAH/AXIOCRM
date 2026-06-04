using AXIOCRM.Application.Interfaces;
using AXIOCRM.Domain.Entities;
using AXIOCRM.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.EventScheduler.Commands.UpdateEvent
{
    public class UpdateEventCommandHandler : IRequestHandler<UpdateEventCommand, bool>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        public UpdateEventCommandHandler(
              AppDbContext context,
              IHttpContextAccessor httpContextAccessor,
              UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var existingEvent = await _context.ScheduleEvents
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (existingEvent == null) return false;

            // Récupération ID utilisateur
            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) throw new UnauthorizedAccessException();

            // Vérification Admin
            var user = await _userManager.FindByIdAsync(currentUserId);
            var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin && existingEvent.ResourceId != currentUserId)
            {
                throw new UnauthorizedAccessException("Vous n'avez pas le droit de modifier cet événement.");
            }

            existingEvent.Title = request.Title;
            existingEvent.Start = request.Start;
            existingEvent.End = request.End;
            existingEvent.ResourceId = isAdmin ? request.ResourceId : currentUserId;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}