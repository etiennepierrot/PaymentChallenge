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

            RuleFor(c => c.CardNumber)
                .Custom((number, context) =>
                {
                    if (!number.IsValid())
                    {
                        context.AddFailure("cardnumber", "Format cardnumber incorrect");
                    };
                    
                });
            
            RuleFor(c => c.Cvv)
                .NotNull().Length(3)
                .OverridePropertyName("cvv")
                .WithMessage("Format cvv incorrect");
            RuleFor(c => c.ExpirationDate)
                .Must( x => _regexExpirationDate.Match(x).Success)
                .OverridePropertyName("expiration_date")
                .WithMessage("Format expiration date incorrect");

        }
    }
}