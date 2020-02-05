using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentChallenge.Domain.Merchants;

namespace PaymentChallenge.Domain.Payments
{
    public interface PaymentRepository
    {
        Task SaveAsync(Payment payment);
        Task<Payment> GetAsync(PaymentId paymentId);
        Task<List<Payment>> GetPaymentsAsync(MerchantId merchantId);
    }
}