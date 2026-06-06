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
        private readonly IIdentityService _identityService;

        public DeleteEventCommandHandler(AppDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }


        public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var ev = await _context.ScheduleEvents.FindAsync(new object[] { request.Id }, cancellationToken);
            if (ev == null) return false;

            var currentUserId = await _identityService.GetCurrentUserIdAsync()
                                        ?? throw new UnauthorizedAccessException();
            
            if (string.IsNullOrEmpty(currentUserId)) throw new UnauthorizedAccessException();

            var isAdmin = await _identityService.IsInRoleAsync(currentUserId, "Admin");

            if (!isAdmin && ev.ResourceId != currentUserId)
                throw new UnauthorizedAccessException("Suppression non autorisée.");

            _context.ScheduleEvents.Remove(ev);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
