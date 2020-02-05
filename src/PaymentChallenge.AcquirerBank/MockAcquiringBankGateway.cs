using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PaymentChallenge.AcquirerBank
{

    public class MockAcquiringBankGateway
    {
        public MockAcquiringBankGateway()
        {
            ForwardedPayments = new Dictionary<string, ResultDto>();
        }

        public static Dictionary<string, ResultDto> ForwardedPayments { get; private set; }

        internal async Task<ResultDto> AuthorizePaymentAsync(BankPaymentDto bankPaymentDto)
        {
            
            if (bankPaymentDto.Reference != null && ForwardedPayments.ContainsKey(bankPaymentDto.Reference))
            {
                return await Task.FromResult(ForwardedPayments[bankPaymentDto.Reference]);
            }
            else
            {
                
                ResultDto resultDto = bankPaymentDto.Amount == 4242 
                    ? new ResultDto("fail", "myPaymentReference") 
                    : new ResultDto("success", "myPaymentReference");

                ForwardedPayments.Add(bankPaymentDto.Reference ?? Guid.NewGuid().ToString(), resultDto);
                if(bankPaymentDto.Amount == 666) 
                    throw new WebException("timeout trigger by special amount for testing", WebExceptionStatus.Timeout);

                return await Task.FromResult(resultDto);

            }
        }
    }
}