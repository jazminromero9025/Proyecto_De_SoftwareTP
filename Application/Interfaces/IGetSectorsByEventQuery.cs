using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Interfaces
{
    public interface IGetSectorsByEventQuery
    {
        Task<List<SectorDto>> Execute(ISectorRepository repository, int eventId);
    }
}
