using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.Persistence
{
    public class InMemoryPaymentRepository : PaymentRepository
    {
        private static readonly Dictionary<PaymentId, Payment> Databag = new Dictionary<PaymentId, Payment>();
        public async Task<Option<Payment>> GetAsync(PaymentId paymentId)
        {
            return await Task.FromResult(Optional(Databag[paymentId]));
        }

        public async Task<Option<Payment>> GetByMerchantReferenceAsync(MerchantId merchantId, Option<MerchantReference> merchantReference)
        {
            return await merchantReference.MatchAsync(async mr =>
            {
                Payment payment = Databag.Values
                    .SingleOrDefault(p => p.MerchantReference == mr
                                          && p.MerchantId == merchantId);
                return await Task.FromResult(Optional(payment));
            }, async () => await Task.FromResult(Option<Payment>.None));
           
        }

        //TODO : add pagination
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
