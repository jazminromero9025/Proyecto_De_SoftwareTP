namespace Application.Models
{
    public class BulkPaymentConfirmationDto
    {
        public List<PaymentConfirmationDto> Confirmations { get; set; } = new();
        public decimal Total { get; set; }
    }
}
