using Application.Interfaces;
using Application.UseCases.Seats.Queries;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;

        public SeatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Seat>> GetSeatsBySectorQueryAsync(GetSeatsBySectorQuery query)
        {
            return await _context.Seats
                .Include(s => s.Reservations)
                .Where(s => s.SectorId == query.SectorId)
                .ToListAsync();
        }
    }
}