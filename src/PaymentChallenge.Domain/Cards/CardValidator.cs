using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace PaymentChallenge.Domain.Cards
{
    public class CardValidator : AbstractValidator<Card>
    {
        public CardValidator()
        {
            Regex regex = new Regex(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$");
            RuleFor(c =>(string) c.CardNumber).SetValidator(new CreditCardValidator());
            RuleFor(c => c.Cvv).NotNull().Length(3).WithMessage("Bad cvv format");
            RuleFor(c => c.ExpirationDate).Must(ed => regex.Match(ed).Success);
        }
    }
}