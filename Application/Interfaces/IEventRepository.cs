using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IEventRepository
    {
        // Definimos el contrato: quien implemente esto DEBE tener un método para traer eventos
        Task<IEnumerable<Event>> GetAllEventsAsync();
    }
}
