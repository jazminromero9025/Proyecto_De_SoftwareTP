using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;



namespace Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            // Trae todos los eventos de la tabla
            return await _context.Events.ToListAsync();
        }




    }
}
