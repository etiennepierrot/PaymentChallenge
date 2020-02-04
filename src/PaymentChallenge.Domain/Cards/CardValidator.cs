using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Validators;

namespace PaymentChallenge.Domain.Cards
{
    public class CardValidator : AbstractValidator<Card>
    {

        private readonly Regex _regexExpirationDate = new Regex(@"(1[2-9]|[2-9][0-9])(0[1-9]|1[0-2])",RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public CardValidator()
        {

            RuleFor(c =>(string) c.CardNumber).SetValidator(new CreditCardValidator())
                .WithMessage("Format Cardnumber incorrect");
            RuleFor(c => c.Cvv).NotNull().Length(3).WithMessage("Format CVV incorrect");
            RuleFor(c => c.ExpirationDate).Must( x => _regexExpirationDate.Match(x).Success)
                .WithMessage("Format Expiration date incorrect");

        }
    }
}