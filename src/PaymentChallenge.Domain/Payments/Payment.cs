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
        public Payment(MerchantId merchantId, Card card, Money money, PaymentId paymentId, PaymentStatus status, MerchantReference merchantReference)
        {
            MerchantId = merchantId;
            Card = card;
            Money = money;
            PaymentId = paymentId;
            Status = status;
            MerchantReference = merchantReference;
        }

        public MerchantId MerchantId { get;  }
        public Card Card { get; }
        public Money Money { get; }
        public PaymentId PaymentId { get; }
        public PaymentStatus Status { get; }
        public string MerchantReference { get; }
    }

    public struct MerchantReference
    {
        private readonly string _merchantReference;

        public MerchantReference(string merchantReference)
        {
            _merchantReference = merchantReference;
        }
        
        public static implicit operator string(MerchantReference merchantReference) => merchantReference._merchantReference;
        public static implicit operator MerchantReference(string str) => new MerchantReference(str);
    }
}