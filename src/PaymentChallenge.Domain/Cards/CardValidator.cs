using FluentValidation;
using FluentValidation.Validators;

namespace PaymentChallenge.Domain.Cards
{
    public class CardValidator : AbstractValidator<Card>
    {
        public CardValidator()
        {
            RuleFor(c =>(string) c.CardNumber).SetValidator(new CreditCardValidator());
            RuleFor(c => c.Cvv).NotNull().Length(3).WithMessage("Bad cvv format");

        }
    }
}