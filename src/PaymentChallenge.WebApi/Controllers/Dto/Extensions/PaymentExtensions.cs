using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.WebApi.Controllers.Dto
{
    public static class PaymentExtensions
    {
        public static PaymentDto ToDto(this Payment payment)
        {
            return new PaymentDto
            {
                Amount =  payment.Amount.ToDto(),
                Card = payment.Card.ToDto(),
                Status = payment.Status.ToString(),
                MerchantReference = payment.MerchantReference
                    .Match(mr => (string) mr, () => ""),
                Id = payment.PaymentId
            };
        }
    }
}