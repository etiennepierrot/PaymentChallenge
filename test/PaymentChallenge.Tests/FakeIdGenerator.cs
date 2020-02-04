using PaymentChallenge.Domain;
using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.Tests
{
    public class FakeIdGenerator : IdGenerator
    {
        private PaymentId _nextPaymentId;

        public FakeIdGenerator()
        {
            _nextPaymentId = "PAY-uniqueId";
        }
        public void NextPaymentId(PaymentId paymentId)
        {
            _nextPaymentId = paymentId;
        }
        public PaymentId GeneratePaymentId()
        {
            return _nextPaymentId;
        }
    }
}