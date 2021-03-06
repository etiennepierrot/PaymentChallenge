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
using LanguageExt.UnitTesting;

namespace PaymentChallenge.Tests
{
    public class PaymentGatewayTests
    {
        private readonly MerchantId _merchantId = new MerchantId("FancyShop");
        private readonly FakeIdGenerator _idGenerator;
        private readonly PaymentGateway _paymentGateway;
        private readonly PaymentRepository _paymentRepository;
        private readonly Card _card = new Card("4242424242424242", "100", "1212");
        private readonly Money _amountPaymentFailedInsufficientFund = new Money(4242, Currency.EUR);
        private readonly Money _amountToCharge = new Money(1000, Currency.EUR);

        public PaymentGatewayTests()
        {
            _idGenerator = new FakeIdGenerator();
            _paymentRepository = new InMemoryPaymentRepository();
            _paymentGateway = new PaymentGateway(_paymentRepository, _idGenerator, new AcquirerBankAdapterImpl(new MockAcquiringBankGateway(), new StubLogger<AcquirerBankAdapterImpl>()));
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
            _idGenerator.NextPaymentId("Forward_A_Payment_To_Acquiring_Bank");
            await _paymentGateway.ProcessPaymentRequestAsync(new PaymentRequest(_card, _merchantId, _amountToCharge));
            MockAcquiringBankGateway.ForwardedPayments.ContainsKey("Forward_A_Payment_To_Acquiring_Bank")
                .Should().BeTrue();
        }
        
        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given an order of a shopper to pay by card
        /// When the Merchant request a payment processing
        /// Then the payment should be forwarded to AcquirerBank with the real card number
        /// </summary>
        [Fact]
        public async Task Forward_A_Payment_To_Acquiring_Bank_With_The_Real_Card_Number()
        {
            _idGenerator.NextPaymentId("Real-Card-Number");
            await _paymentGateway.ProcessPaymentRequestAsync(new PaymentRequest(_card, _merchantId, _amountToCharge));

            MockAcquiringBankGateway.ForwardedPayments["Real-Card-Number"].CardNumber
                .Should().Be("4242424242424242");
        }

        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given an order of a shopper to pay by card
        /// When the Merchant request a payment processing
        /// Then the bank reference of the payment should be persisted
        /// </summary>
        [Fact]
        public async Task Persist_Bank_Reference()
        {
            _idGenerator.NextPaymentId("Persist_Bank_Reference");
            var paymentRequest = new PaymentRequest(_card, _merchantId, _amountToCharge);
            
            var response = await _paymentGateway.ProcessPaymentRequestAsync(paymentRequest);
            await response.IfLeftAsync(async r =>
            {
                var paymentPersisted =  await _paymentRepository.GetAsync(_merchantId, r.PaymentId);
                await paymentPersisted.ShouldBeSome(p => p.BankReference
                    .Should().Be(new AcquirerBankReference("myPaymentReference")));
            });
        }

        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given a network issue (timeout) between Gateway and Acquirer Bank
        /// When the Merchant request a payment processing and the payment has been already been process by the Acquirer Bank
        /// Then the payment should be retry without double charging
        /// </summary>
        [Fact]
        public async Task Idempotency_AcquiringBanking_Side()
        {
            _idGenerator.NextPaymentId("Idempotency_AcquiringBanking_Side");
            var payment = await _paymentGateway.ProcessPaymentRequestAsync(new PaymentRequest(_card, _merchantId, _amountToCharge));
            await payment.ShouldBeLeft(r => r.PaymentStatus.
                Should().Be(PaymentStatus.Approved));
        }

        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given a network issue (timeout) between Merchant and Payment Gateway
        /// And the merchant provider a Merchant Reference
        /// When the Merchant retry to request a payment processing and the payment has been already been process by the Payment Gateway
        /// Then the Payment Gateway return the PaymentResponse of the previous Gateway
        /// </summary>
        [Fact]
        public async Task Idempotency_Merchant_Side()
        {
            PaymentRequest paymentRequest = new PaymentRequest(_card, _merchantId, _amountToCharge, "randomPaymentReference");

            _idGenerator.NextPaymentId("Idempotency_Merchant_Side-1");
            var payment1 = await _paymentGateway.ProcessPaymentRequestAsync(paymentRequest);
            await payment1.IfLeftAsync(async r1 =>
            {
                _idGenerator.NextPaymentId("Idempotency_Merchant_Side-2");
                var payment2 = await _paymentGateway.ProcessPaymentRequestAsync(paymentRequest);
                await payment2.ShouldBeLeft(r2 => r2.Should().Be(r1));
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
            var paymentRequest = new PaymentRequest(_card, _merchantId, _amountPaymentFailedInsufficientFund);
            var paymentResponse = await _paymentGateway.ProcessPaymentRequestAsync(paymentRequest);
            await paymentResponse.ShouldBeLeft(r => r.Should()
                .BeEquivalentTo(new PaymentResponse(PaymentStatus.Refused, "PAY-Return_PaymentStatusFailed")));
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

            var paymentRequest = new PaymentRequest(_card, _merchantId, _amountToCharge);
            var paymentResponse = await _paymentGateway.ProcessPaymentRequestAsync(paymentRequest);

            await paymentResponse.ShouldBeLeft(r =>
                r.Should().BeEquivalentTo(new PaymentResponse(PaymentStatus.Approved, "PAY-Return_PaymentStatus")));

        }


        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given a payment that the merchant has requested 
        /// When the Merchant ask to retrieve this payment 
        /// Then the Merchant should get detailed payment information with masked card number
        /// </summary>
        [Fact]
        public async Task Retrieve_PaymentInfo()
        {
            _idGenerator.NextPaymentId("PAY-Retrieve_PaymentInfo");
            var paymentRequest = new PaymentRequest(_card, _merchantId, _amountToCharge, "ORDER-123");
            var paymentResponse = await _paymentGateway.ProcessPaymentRequestAsync(paymentRequest);

            await paymentResponse.IfLeftAsync(async r =>
            {
                var payment = await _paymentRepository.GetAsync(_merchantId, r.PaymentId);
                await payment.ShouldBeSome( p =>
                {
                    p.Should()
                        .BeEquivalentTo(
                            Payment.CreateFromPaymentRequest(
                                paymentRequest,
                                "PAY-Retrieve_PaymentInfo",
                                new AcquirerBankResponse(PaymentStatus.Approved, "myPaymentReference")));
                });
            });

        }

        /// <summary>
        /// As a Merchant (FancyShop)
        /// Given a payment that the merchant has requested 
        /// When the Merchant ask to retrieve a payment which didn't exist
        /// Then we should return none
        /// </summary>
        [Fact]
        public async Task Retrieve_PaymentInfo_Which_Didnt_Exist()
        {
            var payment = await _paymentRepository.GetAsync(_merchantId, "unknownId");
            await payment.ShouldBeNone();
        }

        /// <summary>
        /// As a Merchant (ConcurrentShop)
        /// Given a payment that an other Shop (FancyShop) has requested 
        /// When ConcurrentShop ask to retrieve the payment of FancyShop
        /// Then we should return none
        /// </summary>
        [Fact]
        public async Task Retrieve_Payment_Of_Another_Merchant()
        {
            _idGenerator.NextPaymentId("PAY-Retrieve_Payment_Of_another_Merchant");
            var paymentRequest = new PaymentRequest(_card, _merchantId, _amountToCharge, "ORDER-Retrieve_Payment_Of_another_Merchant");
            var paymentResponse = await _paymentGateway.ProcessPaymentRequestAsync(paymentRequest);
            await paymentResponse.IfLeft(async p =>
            {
                var payment = await _paymentRepository.GetAsync("ConcurrentShop", p.PaymentId);
                await payment.ShouldBeNone();
            });
        }


        [Fact]
        public async Task Retrieve_List_Payments()
        {
            Make5Payment();
            List<Payment> paymentsRetrieved = await _paymentRepository.GetPaymentsAsync("shop");
            paymentsRetrieved.Count.Should().Be(5);
        }

        private void Make5Payment()
        {
            var tasks = Enumerable.Range(1, 5).Select(async i =>
            {
                _idGenerator.NextPaymentId($"Retrieve_List_Payments-{i}");
                await MakePayment(i.ToString(), "shop");
            }).ToArray();

            Task.WaitAll(tasks);
        }

        private async Task MakePayment(string orderId, MerchantId merchantId)
        {
            _idGenerator.NextPaymentId($"PAY-{orderId}");
            Money amount = new Money(1000, Currency.EUR);
            var paymentRequest = new PaymentRequest(_card, merchantId, amount, orderId);
            await _paymentGateway.ProcessPaymentRequestAsync(paymentRequest);
        }


        [Fact]
        public void MaskedCardNumber()
        {
            Card card = new Card("4242424242424242", "100", "1212");
            ((string) card.CardNumber).Should().Be("4242 XXXX XXXX 4242");
        }


        //  TODO scenario
        //  Error payment (bad cardnumber)
        //  Persist in filesystem
        //  Encrypt Cardnumber ?
        //  and so on ...
        
    }
}