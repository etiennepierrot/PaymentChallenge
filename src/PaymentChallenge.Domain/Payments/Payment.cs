using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.Domain.Payments
{
    public class Payment
    {
        public Payment(MerchantId merchantId, Card card, Money money, PaymentId paymentId, PaymentStatus success)
        {
            MerchantId = merchantId;
            Card = card;
            Money = money;
            PaymentId = paymentId;
            Success = success;
        }

        public MerchantId MerchantId { get;  }
        public Card Card { get; }
        public Money Money { get; }
        public PaymentId PaymentId { get; }
        public PaymentStatus Success { get; }
    }
}