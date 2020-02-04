using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.Tests
{
    public class InMemoryPaymentRepository : PaymentRepository
    {
        private static readonly Dictionary<PaymentId, Payment> _databag = new Dictionary<PaymentId, Payment>();
        public async Task<Payment> GetAsync(PaymentId paymentId)
        {
            return await Task.FromResult(_databag[paymentId]);
        }

        public async Task SaveAsync(Payment payment)
        {
             _databag.Add(payment.PaymentId, payment);
             await Task.CompletedTask;
        }
    }
}
