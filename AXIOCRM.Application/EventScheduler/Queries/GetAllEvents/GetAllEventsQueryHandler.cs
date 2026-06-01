using AXIOCRM.Application.DTOs;
using AXIOCRM.Application.Mappings;
using AXIOCRM.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.EventScheduler.Queries.GetAllEvents
{
    public class GetAllEventsQueryHandler : IRequestHandler<GetAllEventsQuery, List<ScheduleEventDto>>
    {
        private readonly AppDbContext _context;

        public GetAllEventsQueryHandler(AppDbContext context) => _context = context;

        public async Task<List<ScheduleEventDto>> Handle(GetAllEventsQuery request, CancellationToken cancellationToken)
        {
            return await _context.ScheduleEvents
                .AsNoTracking() 
                .Select(e => e.ToDto())
                .ToListAsync(cancellationToken);
        }
    }
}
