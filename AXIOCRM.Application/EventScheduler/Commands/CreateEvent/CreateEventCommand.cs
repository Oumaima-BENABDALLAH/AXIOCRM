using AXIOCRM.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.EventScheduler.Commands.CreateEvent
{
    public record CreateEventCommand(string Title, DateTime Start, DateTime End, string ResourceId) : IRequest<AXIOCRM.Application.DTOs.ScheduleEventDto>;

}
