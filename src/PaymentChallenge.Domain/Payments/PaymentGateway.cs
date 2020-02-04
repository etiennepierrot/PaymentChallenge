using System.Threading.Tasks;
using PaymentChallenge.Domain.AcquiringBank;

namespace PaymentChallenge.Domain.Payments
{
    public class PaymentGateway
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly IdGenerator _idGenerator;
        private readonly AcquiringBankGateway _bankGateway;

        public PaymentGateway(PaymentRepository paymentRepository, IdGenerator idGenerator, AcquiringBankGateway bankGateway)
        {
            _paymentRepository = paymentRepository;
            _idGenerator = idGenerator;
            _bankGateway = bankGateway;
        }

        public async Task<PaymentResponse> ProcessAsync(PaymentRequest command)
        {
            var bankResponse = await _bankGateway.AuthorizePaymentAsync(new BankPaymentDto
            {
                Amount = command.AmountToCharge.Amount,
                Currency = command.AmountToCharge.Currency.ToString(),
                CardNumber = command.Card.CardNumber,
                Cvv = command.Card.Cvv,
                ExpirationDate = command.Card.ExpirationDate
            });
            var paymentId = _idGenerator.GeneratePaymentId();

            Payment payment = new Payment(command.MerchantId, command.Card, command.AmountToCharge, paymentId, bankResponse.Status == "success" ? PaymentStatus.Success : PaymentStatus.Fail, command.MerchantReference );
            await _paymentRepository.SaveAsync(payment);
            return new PaymentResponse(payment.Status, paymentId);
        }

        public async Task<Payment> RetrieveAsync(PaymentId paymentId)
        {
            return await _paymentRepository.GetAsync(paymentId);
        }
    }
}