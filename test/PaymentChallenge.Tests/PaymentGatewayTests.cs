using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using PaymentChallenge.AcquirerBank;
using PaymentChallenge.Domain.AcquiringBank;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Payments;
using PaymentChallenge.Domain.Values;
using PaymentChallenge.Persistence;

namespace PaymentChallenge.Tests
{
    public class PaymentGatewayTests
    {
        private readonly Merchant _merchant;
        private readonly FakeIdGenerator _idGenerator;
        private readonly PaymentGateway _paymentGateway;
        private readonly PaymentRepository _paymentRepository;
        private readonly MockAcquiringBankGateway _acquiringBankGateway = new MockAcquiringBankGateway();
        
        private readonly Card _card = new Card("4242424242424242", "100", "1224" );
        private readonly Money _amountPaymentFailedInsufficientFund = new Money(4242, Currency.EUR);

        public PaymentGatewayTests()
        {
            _merchant = new Merchant("FancyShop");
            _idGenerator = new FakeIdGenerator();
            _paymentRepository = new InMemoryPaymentRepository();
            _paymentGateway = new PaymentGateway(_paymentRepository, _idGenerator, _acquiringBankGateway);
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
            var payment = await _paymentGateway.ProcessAsync(new PaymentRequest(_card, _merchant.Id, amountToCharge));

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
        /// Given an order of a shopper to pay by card with insufficient fund
        /// When the Merchant request a payment processing
        /// Then the Merchant should get a payment identifier and a payment status failed
        /// </summary>
        [Fact]
        public async Task Return_PaymentStatus_Failed()
        {
            _idGenerator.NextPaymentId("PAY-Return_PaymentStatusFailed");
            
            var paymentResponse = await _paymentGateway.ProcessAsync(new PaymentRequest(_card, _merchant.Id, _amountPaymentFailedInsufficientFund) );
            paymentResponse.LeftOrDefault().Should()
                .BeEquivalentTo(new PaymentResponse(PaymentStatus.Fail, "PAY-Return_PaymentStatusFailed"));
        }
        
        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given an order of a shopper to pay by card with an
        /// When the Merchant request a payment processing
        /// Then the Merchant should get a payment identifier and a payment status
        /// </summary>
        [Fact]
        public async Task Return_PaymentStatus()
        {
            _idGenerator.NextPaymentId("PAY-Return_PaymentStatus");
            Money amount = new Money(1000, Currency.EUR);
            
            var paymentResponse = await _paymentGateway.ProcessAsync(new PaymentRequest(_card, _merchant.Id, amount) );
            
            paymentResponse.LeftOrDefault().Should()
                .BeEquivalentTo(new PaymentResponse(PaymentStatus.Success, "PAY-Return_PaymentStatus"));
        }
        
        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given a payment that the merchant has requested 
        /// When the Merchant ask to retrieve this payment 
        /// Then the Merchant should get detailed payment information with Merchant Reference for reconciliation
        /// </summary>
        [Fact]
        public async Task Retrieve_PaymentInfo()
        {
            _idGenerator.NextPaymentId("PAY-Retrieve_PaymentInfo");
            Money amount = new Money(1000, Currency.EUR);
            var paymentResponse = await _paymentGateway.ProcessAsync(new PaymentRequest(_card, _merchant.Id, amount, "ORDER-123") );

            Payment payment = await _paymentRepository.GetAsync(paymentResponse.LeftOrDefault().PaymentId);

            payment.Should()
                .BeEquivalentTo(new Payment(_merchant.Id, _card,
                    new Money(1000, Currency.EUR),
                    "PAY-Retrieve_PaymentInfo", PaymentStatus.Success,
                    "ORDER-123"));
        }
        
        [Fact]
        public async Task Retrieve_List_Payments()
        {
            var payments =  Enumerable.Range(1, 5).ToList().Select(async i =>  await MakePayment(i.ToString()));

            List<Payment> paymentsRetrieved = await _paymentRepository.GetPaymentsAsync(_merchant.Id);

            paymentsRetrieved.Count.Should().Equals(3);
        }

        private async Task MakePayment(string orderId)
        {
            _idGenerator.NextPaymentId($"PAY-{orderId}");
            Money amount = new Money(1000, Currency.EUR);
            var paymentRequest = new PaymentRequest(_card, _merchant.Id, amount, orderId);
            await _paymentGateway.ProcessAsync(paymentRequest);
        }


        //TODO scenario
        //  timeoutexception gateway, retry, and idempotency
        //  Error payment (bad cardnumber, no fund)
        //  Format validation 
        //  List payment merchant
        //  PaymentId not found
        //  Persist in filesystem
        //  cardnumber masked

        //  Encrypt Cardnumber ?
        //  and so on ...
    }
}