using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence; 
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class SectorRepository : ISectorRepository
    {

        private readonly AppDbContext _context;

        public SectorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sector>> GetByEventIdAsync(int eventId)
        {
            return await _context.Sectors
                .Where(s => s.EventId == eventId)
                .ToListAsync();
        }

        public async Task<Sector> CreateSectorAsync(Sector sector)
        {
            _context.Sectors.Add(sector);
            await _context.SaveChangesAsync();
            return sector;
        }




    }
}
