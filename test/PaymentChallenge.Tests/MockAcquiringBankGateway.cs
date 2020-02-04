using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentChallenge.Domain.AcquiringBank;

namespace PaymentChallenge.Tests
{
    public class MockAcquiringBankGateway : AcquiringBankGateway
    {
        public MockAcquiringBankGateway()
        {
            ForwardedPayments = new List<BankPaymentDto>();
        }

        public static List<BankPaymentDto> ForwardedPayments { get; private set; }
        public async Task<ResultDto> AuthorizePaymentAsync(BankPaymentDto bankPaymentDto)
        {
            ForwardedPayments.Add(bankPaymentDto);
            return await Task.FromResult(new ResultDto("success", "myPaymentReference"));
        }
    }
}