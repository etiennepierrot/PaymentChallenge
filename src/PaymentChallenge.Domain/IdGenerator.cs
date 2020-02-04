using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.Domain
{
    public interface IdGenerator
    {
        PaymentId GeneratePaymentId();
    }
}