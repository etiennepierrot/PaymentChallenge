using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentChallenge.Domain.AcquiringBank;
using PaymentChallenge.Domain.Payments;
using Polly;
using LanguageExt;
using static LanguageExt.Prelude;

namespace PaymentChallenge.AcquirerBank
{
    public class AcquirerBankAdapterImpl : AcquirerBankAdapter
    {
        private readonly MockAcquiringBankGateway _acquiringBankGateway;
        private readonly ILogger<AcquirerBankAdapterImpl> _logger;

        public AcquirerBankAdapterImpl(
            MockAcquiringBankGateway acquiringBankGateway, 
            ILogger<AcquirerBankAdapterImpl> logger)
        {
            _acquiringBankGateway = acquiringBankGateway;
            _logger = logger;
        }

        public async Task<AcquirerBankResponse> BankResponse(PaymentRequest command, PaymentId paymentId)
        {

            var policy = Policy
                .Handle<WebException>()
                .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(1));

            ResultDto dto = await policy.ExecuteAsync(async () =>
            {
                LogInput(command, paymentId);
                var resultDto = await CallBank(command, paymentId);
                LogOuput(resultDto);
                return resultDto;
            });

            PaymentStatus status = dto.Status == "success" ? PaymentStatus.Success : PaymentStatus.Fail;
            return new AcquirerBankResponse( status, dto.PaymentReference);
        }

        private async Task<ResultDto> CallBank(PaymentRequest command, PaymentId paymentId)
        {
            var resultDto = await _acquiringBankGateway.AuthorizePaymentAsync(new BankPaymentDto
            {
                Amount = command.AmountToCharge.Amount,
                Currency = command.AmountToCharge.Currency.ToString(),
                CardNumber = command.Card.CardNumber,
                Cvv = command.Card.Cvv,
                ExpirationDate = command.Card.ExpirationDate,
                Reference = paymentId
            });
            return resultDto;
        }


        private Unit LogOuput(ResultDto resultDto)
        {
            _logger.Log(LogLevel.Information,
                $"Response Acquirer Bank : " +
                $"Status : {resultDto.Status} " +
                $"PaymentReference : {resultDto.PaymentReference}");
            return unit;
        }

        private void LogInput(PaymentRequest command, PaymentId paymentId)
        {
            _logger.Log(LogLevel.Information,
                $"Call Acquirer Bank : " +
                $"Amount : {command.AmountToCharge.Amount} {command.AmountToCharge.Currency.ToString()}" +
                $"Reference : {paymentId}");
        }
    }
}