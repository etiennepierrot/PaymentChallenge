using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.Persistence
{
    public class InMemoryPaymentRepository : PaymentRepository
    {
        private static readonly Dictionary<PaymentId, Payment> Databag = new Dictionary<PaymentId, Payment>();
        public async Task<Payment> GetAsync(PaymentId paymentId)
        {
            return await Task.FromResult(Databag[paymentId]);
        }

        public async Task<List<Payment>> GetPaymentsAsync(MerchantId merchantId)
        {
            return await Task.FromResult(Databag.Values
                .Where(p => p.MerchantId == merchantId)
                .ToList());
        }

        public async Task SaveAsync(Payment payment)
        {
             Databag.Add(payment.PaymentId, payment);
             await Task.CompletedTask;
        }
    }
}
