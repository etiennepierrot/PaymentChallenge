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
        private PaymentRequestValidator _validator;
        private readonly Merchant _merchant = new Merchant("FancyShop");
        private readonly Card _invalidCard = new Card("42424242424242427", "1000", "12245" );


        public ValidatorPaymentRequestTest()
        {
            _validator = new PaymentRequestValidator();
        }

        [Fact]
        public void ValidationCardNumber()
        {
            Money amount = new Money(1000, Currency.EUR);

            TestValidationResult<PaymentRequest, PaymentRequest> validationResult = _validator.TestValidate(new PaymentRequest(_invalidCard, _merchant.Id, amount, "ORDER-123"));
            validationResult.ShouldHaveValidationErrorFor(pr => pr.Card.CardNumber);
            validationResult.ShouldHaveValidationErrorFor(pr => pr.Card.Cvv);
            validationResult.ShouldHaveValidationErrorFor(pr => pr.Card.ExpirationDate);
        }

        [Fact]
        public void ValidationStriclyPositveAmount()
        {
            Money amount = new Money(0, Currency.EUR);

            _validator.TestValidate(new PaymentRequest(_invalidCard, _merchant.Id, amount, "ORDER-123"))
                .ShouldHaveValidationErrorFor(pr => pr.AmountToCharge);
        }
    }
}