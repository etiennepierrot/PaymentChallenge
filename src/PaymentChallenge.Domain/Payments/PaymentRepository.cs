using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using PaymentChallenge.Domain.Merchants;

namespace PaymentChallenge.Domain.Payments
{
    public interface PaymentRepository
    {
        Task SaveAsync(Payment payment);
        Task<OptionAsync<Payment>> GetAsync(MerchantId merchantId, PaymentId paymentId);
        Task<OptionAsync<Payment>> GetByMerchantReferenceAsync(MerchantId merchantId, Option<MerchantReference> merchantReference);
        Task<List<Payment>> GetPaymentsAsync(MerchantId merchantId);
    }
}