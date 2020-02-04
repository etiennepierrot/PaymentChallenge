using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using PaymentChallenge.Domain.AcquiringBank;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Payments;
using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.Tests
{
    public class PaymentGatewayTests
    {
        private readonly Merchant _merchant;
        private readonly FakeIdGenerator _idGenerator;
        private readonly PaymentGateway _paymentGateway;
        private readonly MockAcquiringBankGateway _acquiringBankGateway = new MockAcquiringBankGateway();
        
        private readonly Card _card = new Card("4242424242424242", "100", "1224" );

        public PaymentGatewayTests()
        {
            _merchant = new Merchant("FancyShop");
            _idGenerator = new FakeIdGenerator();
            _paymentGateway = new PaymentGateway(new InMemoryPaymentRepository(), _idGenerator, _acquiringBankGateway);
        }

        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given an order of a shopper to pay by card
        /// When the Merchant request a payment processing
        /// Then the payment should be forwarded to AcquirerBank
        /// </summary>
        [Fact]
        public async Task Forward_A_Payment_To_Acquiring_Bank()
        {
            var amountToCharge = new Money(1000, Currency.EUR);
            PaymentResponse payment = await _paymentGateway.ProcessAsync(new PaymentRequest(_card, _merchant.Id, amountToCharge));

            MockAcquiringBankGateway.ForwardedPayments.Should()
                .BeEquivalentTo(new List<BankPaymentDto>()
                {
                    new BankPaymentDto()
                    {
                        Amount = 1000,
                        Currency = "EUR",
                        CardNumber = "4242424242424242",
                        Cvv = "100",
                        ExpirationDate = "1224"
                    }
                });    

        }
        
        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given an order of a shopper to pay by card
        /// When the Merchant request a payment processing
        /// Then the Merchant should get a payment identifier and a payment status
        /// </summary>
        [Fact]
        public async Task Return_PaymentStatus()
        {
            _idGenerator.NextPaymentId("PAY-Return_PaymentStatus");
            Money amount = new Money(1000, Currency.EUR);
            
            PaymentResponse paymentResponse = await _paymentGateway.ProcessAsync(new PaymentRequest(_card, _merchant.Id, amount) );
            
            paymentResponse.Should()
                .BeEquivalentTo(new PaymentResponse(PaymentStatus.Success, "PAY-Return_PaymentStatus"));
        }
        
        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given a payment that the merchant has requested 
        /// When the Merchant ask to retrieve this payment 
        /// Then the Merchant should get detailed payment information
        /// </summary>
        [Fact]
        public async Task Retrieve_PaymentInfo()
        {
            _idGenerator.NextPaymentId("PAY-Retrieve_PaymentInfo");
            Money amount = new Money(1000, Currency.EUR);
            PaymentResponse paymentResponse = await _paymentGateway.ProcessAsync(new PaymentRequest(_card, _merchant.Id, amount) );

            Payment payment = await _paymentGateway.RetrieveAsync(paymentResponse.PaymentId);
            
            payment.Should()
                .BeEquivalentTo(new Payment(_merchant.Id, _card, new Money(1000, Currency.EUR), "PAY-Retrieve_PaymentInfo", PaymentStatus.Success));
        }
    }
}