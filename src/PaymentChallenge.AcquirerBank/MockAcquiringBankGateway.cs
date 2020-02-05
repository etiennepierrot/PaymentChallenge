using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using PaymentChallenge.Domain.AcquiringBank;
using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.AcquirerBank
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
            if(bankPaymentDto.Amount == 666) throw  new WebException("", WebExceptionStatus.Timeout);

            ForwardedPayments.Add(bankPaymentDto);
            return await Task.FromResult(bankPaymentDto.Amount == 4242 
                ? new ResultDto("fail", "myPaymentReference") 
                : new ResultDto("success", "myPaymentReference"));

        }

       
    }
}