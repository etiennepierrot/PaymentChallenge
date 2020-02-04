using System.Threading.Tasks;

namespace PaymentChallenge.Domain.Payments
{
    public interface PaymentRepository
    {
        Task SaveAsync(Payment payment);
        Task<Payment> GetAsync(PaymentId paymentId);
    }
}