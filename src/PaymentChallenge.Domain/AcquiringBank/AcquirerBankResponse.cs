using PaymentChallenge.Domain.Payments;

namespace PaymentChallenge.Domain.AcquiringBank
{
    public class AcquirerBankResponse
    {
        public AcquirerBankResponse(PaymentStatus status, AcquirerBankReference bankReference)
        {
            Status = status;
            BankReference = bankReference;
        }

        public PaymentStatus Status { get; }
        public AcquirerBankReference BankReference { get; }

    }
}