using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation.Results;
using PaymentChallenge.Domain.AcquiringBank;

namespace PaymentChallenge.Domain.Payments
{
    public class PaymentGateway
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly IdGenerator _idGenerator;
        private readonly AcquiringBankGateway _bankGateway;
        private PaymentRequestValidator _paymentRequestValidator;

        public PaymentGateway(PaymentRepository paymentRepository, IdGenerator idGenerator, AcquiringBankGateway bankGateway)
        {
            _paymentRepository = paymentRepository;
            _idGenerator = idGenerator;
            _bankGateway = bankGateway;
            _paymentRequestValidator = new PaymentRequestValidator();
        }

        public async Task<Either<PaymentResponse, ValidationResult>> ProcessAsync(PaymentRequest command)
        {
            ValidationResult validationResult = _paymentRequestValidator.Validate(command);
            if (!validationResult.IsValid) return new Either<PaymentResponse, ValidationResult>(validationResult);

            var paymentId = _idGenerator.GeneratePaymentId();
            ResultDto bankResponse;
            try
            {
                bankResponse = await _bankGateway.AuthorizePaymentAsync(new BankPaymentDto
                {
                    Amount = command.AmountToCharge.Amount,
                    Currency = command.AmountToCharge.Currency.ToString(),
                    CardNumber = command.Card.CardNumber,
                    Cvv = command.Card.Cvv,
                    ExpirationDate = command.Card.ExpirationDate,
                    Reference = paymentId
                });
            }
            catch (WebException e)
            {
                //TODO Add logging
                bankResponse = await _bankGateway.RetrieveAuthorization(paymentId);
            }


            PaymentStatus paymentStatus = bankResponse.Status == "success" ? PaymentStatus.Success : PaymentStatus.Fail;
            Payment payment = new Payment(command.MerchantId, command.Card, command.AmountToCharge, paymentId, paymentStatus, command.MerchantReference);
            await _paymentRepository.SaveAsync(payment);
            return new Either<PaymentResponse, ValidationResult>(new PaymentResponse(payment.Status, paymentId));

        }

        public async Task<Payment> RetrieveAsync(PaymentId paymentId)
        {
            return await _paymentRepository.GetAsync(paymentId);
        }
    }
}