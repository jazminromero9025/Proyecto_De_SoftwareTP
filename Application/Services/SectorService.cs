using Application.Interfaces;
using Application.Models;
using Application.UseCases.Sectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SectorService : ISectorService
    {
        private readonly ISectorRepository _sectorRepository;

        public SectorService(ISectorRepository sectorRepository)
        {
            _sectorRepository = sectorRepository;
        }



        public async Task<List<SectorDto>> GetSectorsByEventAsync(int eventId)
        {
            // Usamos la interfaz para la variable, pero instanciamos la clase
            IGetSectorsByEventQuery query = new GetSectorsByEventQuery();

            return await query.Execute(_sectorRepository, eventId);
        }


    }
}
