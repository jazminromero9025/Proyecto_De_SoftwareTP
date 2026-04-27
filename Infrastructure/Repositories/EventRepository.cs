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
            // 1. Usamos Entity Framework para traer la lista de la DB
            
            // le dice a EF que no necesita seguir estos objetos porque solo son de lectura.
            return await _context.Events
                .AsNoTracking()
                .ToListAsync();

        }


        public async Task AddAsync(Event newEvent)
        {
            // 1. Agregamos el objeto al set de Eventos
            await _context.Events.AddAsync(newEvent);

            // Guardamos los cambios en la base de datos
            await _context.SaveChangesAsync();
        }





    }
}
