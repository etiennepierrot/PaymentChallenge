using System.Threading.Tasks;
using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.Domain.AcquiringBank
{
    public interface AcquiringBankGateway
    {
        /// <exception cref="System.Net.WebException"></exception>
        Task<ResultDto> AuthorizePaymentAsync(BankPaymentDto bankPaymentDto);

        Task<ResultDto> RetrieveAuthorization(PaymentId paymentId);
    }
}