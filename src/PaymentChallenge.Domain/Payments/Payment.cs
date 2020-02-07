using LanguageExt;
using PaymentChallenge.Domain.AcquiringBank;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.Domain.Payments
{
    /// <summary>
    /// In real production application this kind of class can't be a value object
    /// because this state could mutate (ex : a waiting status who wait for a success of failure).
    /// But for the moment any use case require a mutation
    /// so i decide to stay immutable because I like immutability
    /// </summary>
    public class Payment
    {
        public static Payment CreateFromPaymentRequest(PaymentRequest paymentRequest, PaymentId paymentId, AcquirerBankResponse bankResponse)
        {
            return new Payment()
            {
                MerchantId = paymentRequest.MerchantId,
                Card = paymentRequest.Card,
                Amount = paymentRequest.AmountToCharge,
                MerchantReference = paymentRequest.MerchantReference,
                PaymentId = paymentId,
                Status = bankResponse.Status,
                BankReference = bankResponse.BankReference,
            };
        }


        public MerchantId MerchantId { get; private set; }
        public Card Card { get;  private set;  }
        public Money Amount { get;  private set; }
        public PaymentId PaymentId { get;  private set; }
        public PaymentStatus Status { get;  private set; }
        public Option<MerchantReference> MerchantReference { get;  private set; }
        public AcquirerBankReference BankReference { get; private set; }
    }
}