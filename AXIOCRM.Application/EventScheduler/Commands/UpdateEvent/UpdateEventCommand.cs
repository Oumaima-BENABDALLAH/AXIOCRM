using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.EventScheduler.Commands.UpdateEvent
{
    public record UpdateEventCommand(
     int Id,
     string Title,
     DateTime Start,
     DateTime End,
     string ResourceId) : IRequest<bool>;
}
