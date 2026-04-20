using Domain.Entities;
using Application.Interfaces;
using Infrastructure.Persistence;
using Application.UseCases.Seats.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
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
                .Where(s => s.SectorId == query.SectorId)
                .ToListAsync();
        }





    }
}
