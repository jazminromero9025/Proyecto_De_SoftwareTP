using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.UseCases.Reservations.Commands;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateReservationAsync(CreateReservationCommand command);
        Task CreateAuditLogAsync(CreateAuditLogCommand command);

    }
}
