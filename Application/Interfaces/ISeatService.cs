using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Models;
using Application.UseCases.Seats.Queries;

namespace Application.Interfaces
{
    public interface ISeatService
    {
        Task<List<SeatDto>> GetSeatsBySectorAsync(Guid sectorId);


    }
}
