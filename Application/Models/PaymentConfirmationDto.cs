namespace Application.Models
{
    public class PaymentConfirmationDto
    {
        public Guid ReservationId { get; set; }
        public Guid SeatId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PaidAt { get; set; }
    }
}
