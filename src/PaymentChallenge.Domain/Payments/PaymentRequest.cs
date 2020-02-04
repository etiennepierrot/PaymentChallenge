using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.Domain.Payments
{
    public struct PaymentRequest
    {
        public PaymentRequest(Card card, MerchantId merchantId, Money amountToCharge)
        {
            Card = card;
            MerchantId = merchantId;
            AmountToCharge = amountToCharge;
        }

        public Card Card { get; }
        public MerchantId  MerchantId { get; }
        public Money AmountToCharge { get; }
    }
}