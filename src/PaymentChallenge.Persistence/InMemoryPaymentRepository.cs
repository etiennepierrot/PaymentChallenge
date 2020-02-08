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
        
        
        public async Task<OptionAsync<Payment>> GetAsync(MerchantId merchantId, PaymentId paymentId)
        {
            if (!Databag.ContainsKey(paymentId)) return OptionAsync<Payment>.None;
            Payment payment = Databag[paymentId];
            if(payment.MerchantId != merchantId) return OptionAsync<Payment>.None;
            return await Task.FromResult(OptionAsync<Payment>.Some(payment));
        }

        public async Task<OptionAsync<Payment>> GetByMerchantReferenceAsync(MerchantId merchantId, Option<MerchantReference> merchantReference)
        {
            return await merchantReference.Match(async mr =>
            {
                Payment payment = Databag.Values.SingleOrDefault(p => p.MerchantReference == mr
                                                                      && p.MerchantId == merchantId);
                if (payment == null) return await Task.FromResult(OptionAsync<Payment>.None);
                return await Task.FromResult(OptionAsync<Payment>.Some(payment));
            }, 
                async () => await Task.FromResult(OptionAsync<Payment>.None));
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
