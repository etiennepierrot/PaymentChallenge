using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public class PaymentResponseDto
    {
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentId { get; set; }
    }
}