using System.Threading.Tasks;

namespace PaymentChallenge.Domain.AcquiringBank
{
    public interface AcquiringBankGateway
    {
        /// <exception cref="System.Net.WebException"></exception>
        Task<ResultDto> AuthorizePaymentAsync(BankPaymentDto bankPaymentDto);

    }
}