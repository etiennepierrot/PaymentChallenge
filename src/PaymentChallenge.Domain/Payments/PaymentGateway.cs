using System.Threading.Tasks;
using FluentValidation.Results;
using LanguageExt;
using PaymentChallenge.Domain.AcquiringBank;
using static LanguageExt.Prelude;

namespace PaymentChallenge.Domain.Payments
{
    public class PaymentGateway
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly IdGenerator _idGenerator;
        private readonly PaymentRequestValidator _paymentRequestValidator;
        private readonly AcquirerBankAdapter _acquirerBankAdapter;

        public PaymentGateway(PaymentRepository paymentRepository, IdGenerator idGenerator, AcquirerBankAdapter acquirerBankAdapter)
        {
            _paymentRepository = paymentRepository;
            _idGenerator = idGenerator;
            _paymentRequestValidator = new PaymentRequestValidator();
            _acquirerBankAdapter = acquirerBankAdapter;
        }

        public async Task<Either<PaymentResponse, ValidationResult>> ProcessPaymentRequestAsync(PaymentRequest command)
        {
            ValidationResult validationResult = _paymentRequestValidator.Validate(command);
            if (!validationResult.IsValid) return Right(validationResult);
            return Left(await MakePayment(command));
        }

        private async Task<PaymentResponse> MakePayment(PaymentRequest command)
        {
            var payment = await _paymentRepository.GetByMerchantReferenceAsync(command.MerchantId, command.MerchantReference);
            return await payment.MatchAsync(p => new PaymentResponse(p.Status, p.PaymentId),
                async () =>
                {
                    var paymentId = _idGenerator.GeneratePaymentId();
                    var bankResponse = await _acquirerBankAdapter.BankResponse(command, paymentId);
                    await CreatePayment(command, paymentId, bankResponse);
                    return new PaymentResponse(bankResponse.Status, paymentId);
                });
        }

        private async Task CreatePayment(PaymentRequest command, PaymentId paymentId, AcquirerBankResponse bankResponse)
        {
            Payment payment = Payment.CreateFromPaymentRequest(command, paymentId, bankResponse);
            //TODO unit of work pattern if we need to make other change in our future DB
            await _paymentRepository.SaveAsync(payment);
        }
    }
}