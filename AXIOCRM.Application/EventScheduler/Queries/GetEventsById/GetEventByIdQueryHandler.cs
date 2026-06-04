using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Interfaces;
using AXIOCRM.Application.Mappings;
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

namespace AXIOCRM.Application.EventScheduler.Queries.GetEventsById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventByIdQuery, ScheduleEventDto?>
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetEventByIdQueryHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<ScheduleEventDto?> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var ev = await _context.ScheduleEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

            if (ev == null) return null;

            var currentUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId)) throw new UnauthorizedAccessException();

            var user = await _userManager.FindByIdAsync(currentUserId);
            var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin && ev.ResourceId != currentUserId)
                throw new UnauthorizedAccessException("Accès non autorisé à cet événement.");

            return ev.ToDto();
        }
    }
}
