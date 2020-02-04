using System.Threading.Tasks;

namespace PaymentChallenge.Domain.AcquiringBank
{
    public interface AcquiringBankGateway
    {
        Task<ResultDto> AuthorizePaymentAsync(BankPaymentDto bankPaymentDto);
    }
}