using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using PaymentChallenge.Domain.Merchants;

namespace PaymentChallenge.Domain.Payments
{
    public interface PaymentRepository
    {
        Task SaveAsync(Payment payment);
        Task<Option<Payment>> GetAsync(MerchantId merchantId, PaymentId paymentId);
        Task<Option<Payment>> GetByMerchantReferenceAsync(MerchantId merchantId, Option<MerchantReference> merchantReference);
        Task<List<Payment>> GetPaymentsAsync(MerchantId merchantId);
    }
}