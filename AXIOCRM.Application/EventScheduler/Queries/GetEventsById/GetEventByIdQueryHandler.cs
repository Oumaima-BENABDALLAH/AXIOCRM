using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.EventScheduler.Queries.GetEventsById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, ScheduleEventDto?>
    {
        private readonly AppDbContext _context;
        private readonly IIdentityService _identityService;

        public GetEventByIdQueryHandler(AppDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<ScheduleEventDto?> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var ev = await _context.ScheduleEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (ev == null) return null;

            var currentUserId = await _identityService.GetCurrentUserIdAsync();
            var isAdmin = await _identityService.IsInRoleAsync(currentUserId!, "Admin");

            if (!isAdmin && ev.ResourceId != currentUserId)
                throw new UnauthorizedAccessException("Accès non autorisé à cet événement.");

            return ev.ToDto();
        }
    }
}
