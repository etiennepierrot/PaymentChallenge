using LanguageExt;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Values;
using static LanguageExt.Prelude;

namespace PaymentChallenge.Domain.Payments
{
    public class PaymentRequest
    {
        public PaymentRequest(Card card, MerchantId merchantId, Money amountToCharge, MerchantReference merchantReference = default)
        {
            Card = card;
            MerchantId = merchantId;
            AmountToCharge = amountToCharge;
            MerchantReference = merchantReference == default(MerchantReference) ? Option<MerchantReference>.None : Optional(merchantReference) ;
        }

        public Card Card { get; }
        public MerchantId  MerchantId { get; }
        public Money AmountToCharge { get; }
        public Option<MerchantReference> MerchantReference { get; }

    }
}