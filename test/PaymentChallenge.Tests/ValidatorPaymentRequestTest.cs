using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentValidation.TestHelper;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Payments;
using PaymentChallenge.Domain.Values;
using Xunit;

namespace PaymentChallenge.Tests
{
    public class ValidatorPaymentRequestTest
    {

        private readonly PaymentRequestValidator _validator;
        private readonly CardValidator _cardValidator;
        private readonly MerchantId _merchantId = "FancyShop";
        private readonly Card _invalidCard = new Card("42424242424242427", "1000", "1213" );


        public ValidatorPaymentRequestTest()
        {
            _validator = new PaymentRequestValidator();
            _cardValidator = new CardValidator();
        }

        [Fact]
        public void ValidationCardNumber()
        {
            var validationResult = _cardValidator.Validate(_invalidCard);

            validationResult.Errors.Select(e => e.PropertyName
            ).Should().BeEquivalentTo(new List<string>
            {
                "cardnumber", 
                "cvv", 
                "expiration_date"
            });

        }

        [Fact]
        public void ValidationStriclyPositveAmount()
        {
            Money amount = new Money(0, Currency.EUR);

            _validator.TestValidate(new PaymentRequest(_invalidCard, _merchantId, amount, "ORDER-123"))
                .ShouldHaveValidationErrorFor(pr => pr.AmountToCharge);
        }
    }
}