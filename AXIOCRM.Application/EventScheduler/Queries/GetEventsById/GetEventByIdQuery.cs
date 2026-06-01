using AXIOCRM.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.EventScheduler.Queries.GetEventsById
{
    public record GetEventByIdQuery(int Id) : IRequest<ScheduleEventDto?>;
}
