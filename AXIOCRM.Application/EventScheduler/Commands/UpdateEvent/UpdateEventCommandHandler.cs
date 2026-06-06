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
        private readonly IIdentityService _identityService;
        public UpdateEventCommandHandler(AppDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var existingEvent = await _context.ScheduleEvents
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (existingEvent == null) return false;

            // Récupération ID utilisateur
            var currentUserId = await _identityService.GetCurrentUserIdAsync()
                                        ?? throw new UnauthorizedAccessException();

            if (string.IsNullOrEmpty(currentUserId)) throw new UnauthorizedAccessException();

            // Vérification Admin
            var isAdmin = await _identityService.IsInRoleAsync(currentUserId, "Admin");

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