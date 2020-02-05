using System.Threading.Tasks;
using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.Domain.AcquiringBank
{
    public interface AcquirerBankAdapter
    {
        Task<AcquirerBankResponse> BankResponse(PaymentRequest command, PaymentId paymentId);
    }
}