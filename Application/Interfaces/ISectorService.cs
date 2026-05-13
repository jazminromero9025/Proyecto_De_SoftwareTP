using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;

namespace Application.Interfaces
{
    public interface ISectorService
    {
        Task<List<SectorDto>> GetSectorsByEventAsync(int eventId);
        Task<SectorDto> CreateSectorAsync(int eventId, string name, decimal price, int capacity);
    }
}
