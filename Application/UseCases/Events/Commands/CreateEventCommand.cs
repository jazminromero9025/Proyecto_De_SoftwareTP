using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Application.UseCases.Events.Commands
{
    public record CreateEventCommand(
        string Name,
        DateTime EventDate,
        string Venue
    ) : ICreateEventCommand;
    
    
}
