using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AXIOCRM.Application.EventScheduler.Commands.DeleteEvent
{
    public record DeleteEventCommand(int Id) : IRequest<bool>;
}
