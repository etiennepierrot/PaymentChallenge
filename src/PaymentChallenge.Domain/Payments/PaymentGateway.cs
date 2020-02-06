using System.Threading.Tasks;
using FluentValidation.Results;
using PaymentChallenge.Domain.AcquiringBank;

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

        public async Task<Either<PaymentResponse, ValidationResult>> ProcessAsync(PaymentRequest command)
        {
           

            ValidationResult validationResult = _paymentRequestValidator.Validate(command);
            if (!validationResult.IsValid) return new Either<PaymentResponse, ValidationResult>(validationResult);

            var paymentId = _idGenerator.GeneratePaymentId();

            var bankResponse = await _acquirerBankAdapter.BankResponse(command, paymentId);

            Payment payment = Payment.CreateFromPaymentRequest(command, paymentId, bankResponse);
            
            //TODO unit of work pattern if we need to make other change in our future DB
            await _paymentRepository.SaveAsync(payment);
            return new Either<PaymentResponse, ValidationResult>(new PaymentResponse(payment.Status, paymentId));

        }

    }
}