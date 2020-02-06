using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.Domain.Payments
{
    public class PaymentRequest
    {
        public PaymentRequest(Card card, MerchantId merchantId, Money amountToCharge, string paymentReference = "")
        {
            Card = card;
            MerchantId = merchantId;
            AmountToCharge = amountToCharge;
            MerchantReference = paymentReference;
        }

        public Card Card { get; }
        public MerchantId  MerchantId { get; }
        public Money AmountToCharge { get; }
        public MerchantReference MerchantReference { get; }

    }
}