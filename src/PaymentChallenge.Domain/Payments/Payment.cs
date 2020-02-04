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
    public struct Payment
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