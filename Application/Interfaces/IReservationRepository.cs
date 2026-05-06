using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Models;
using Application.UseCases.Reservations.Commands;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateReservationAsync(CreateReservationCommand command);
        Task<PaymentConfirmationDto> ConfirmPaymentAsync(Guid reservationId);
        Task<BulkPaymentConfirmationDto> ConfirmBulkPaymentAsync(List<Guid> reservationIds);
    }
}
