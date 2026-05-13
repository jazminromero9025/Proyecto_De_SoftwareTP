using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISectorRepository
    {
        Task<List<Sector>> GetByEventIdAsync(int eventId);
        Task<Sector> CreateSectorAsync(Sector sector);

    }
}
