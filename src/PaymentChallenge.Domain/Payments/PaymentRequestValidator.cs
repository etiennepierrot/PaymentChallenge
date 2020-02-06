using FluentValidation;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.Domain.Payments
{
    public class PaymentRequestValidator : AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(c => c.Card).SetValidator(new CardValidator());
            RuleFor(c => c.AmountToCharge).Must(x => x > new Money(0, x.Currency));
        }
    }
}