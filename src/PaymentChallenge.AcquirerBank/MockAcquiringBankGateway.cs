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
            Results = new Dictionary<string, ResultDto>();
            ForwardedPayments = new Dictionary<string, BankPaymentDto>();
        }
        public static Dictionary<string, ResultDto> F { get; private set; }

        public static Dictionary<string, BankPaymentDto> ForwardedPayments { get; private set; }
        public static Dictionary<string, ResultDto> Results { get; private set; }


        internal async Task<ResultDto> AuthorizePaymentAsync(BankPaymentDto bankPaymentDto)
        {
            if (IdempotencyCheck(bankPaymentDto))
            {
                return await Task.FromResult(Results[bankPaymentDto.Reference]);
            }

            ForwardedPayments.Add(bankPaymentDto.Reference, bankPaymentDto);
            ResultDto resultDto = bankPaymentDto.Amount == 4242 
                ? new ResultDto("fail", "myPaymentReference") 
                : new ResultDto("success", "myPaymentReference");
            Results.Add(bankPaymentDto.Reference ?? Guid.NewGuid().ToString(), resultDto);

            if(bankPaymentDto.Amount == 666) 
                throw new WebException("timeout trigger by special amount for testing", WebExceptionStatus.Timeout);

            return await Task.FromResult(resultDto);
        }

        private static bool IdempotencyCheck(BankPaymentDto bankPaymentDto)
        {
            return bankPaymentDto.Reference != null && ForwardedPayments.ContainsKey(bankPaymentDto.Reference);
        }
    }
}