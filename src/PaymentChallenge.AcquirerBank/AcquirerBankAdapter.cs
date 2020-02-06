using System;
using System.Net;
using System.Threading.Tasks;
using PaymentChallenge.Domain.AcquiringBank;
using PaymentChallenge.Domain.Payments;
using Polly;

namespace PaymentChallenge.AcquirerBank
{
    public class AcquirerBankAdapterImpl : AcquirerBankAdapter
    {
        private readonly MockAcquiringBankGateway _acquiringBankGateway;

        public AcquirerBankAdapterImpl(MockAcquiringBankGateway acquiringBankGateway)
        {
            _acquiringBankGateway = acquiringBankGateway;
        }

        public async Task<AcquirerBankResponse> BankResponse(PaymentRequest command, PaymentId paymentId)
        {

            var policy = Policy
                .Handle<WebException>()
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(1));

            ResultDto dto = await policy.ExecuteAsync( (async () => await 
                //TODO we need to to log the input and the ouput and filter the cardnumber for obvious privacy reason
                _acquiringBankGateway.AuthorizePaymentAsync(new BankPaymentDto
            {
                Amount = command.AmountToCharge.Amount,
                Currency = command.AmountToCharge.Currency.ToString(),
                CardNumber = command.Card.CardNumber,
                Cvv = command.Card.Cvv,
                ExpirationDate = command.Card.ExpirationDate,
                Reference = paymentId
            })));
            return new AcquirerBankResponse( dto.Status == "success" ? PaymentStatus.Success : PaymentStatus.Fail, dto.PaymentReference);
        }
    }
}