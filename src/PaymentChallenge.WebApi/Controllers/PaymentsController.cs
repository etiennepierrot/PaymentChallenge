using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentChallenge.Domain;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Merchants;
using PaymentChallenge.Domain.Payments;
using PaymentChallenge.Domain.Values;

namespace PaymentChallenge.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ILogger<PaymentsController> _logger;
        private readonly PaymentGateway _paymentGateway;
        private readonly PaymentRepository _paymentRepository;

        public PaymentsController(ILogger<PaymentsController> logger, PaymentGateway paymentGateway, PaymentRepository paymentRepository)
        {
            _logger = logger;
            _paymentGateway = paymentGateway;
            _paymentRepository = paymentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequestDto paymentRequest)
        {
            var payment = await ToPaymentRequestModel(paymentRequest);
            return payment.Match(response => Ok(new PaymentResponseDto
                {
                    PaymentId = response.PaymentId,
                    PaymentStatus = response.PaymentStatus
                }),
                result => Problem(result.Errors.ToString()));
        }

        private async Task<Either<PaymentResponse, ValidationResult>> ToPaymentRequestModel(PaymentRequestDto paymentRequest)
        {
            var card = new Card(paymentRequest.Card.CardNumber, paymentRequest.Card.Cvv, paymentRequest.Card.ExpirationDate);
            var payment = await _paymentGateway.ProcessAsync(new PaymentRequest(card, paymentRequest.MerchantId,
                new Money(paymentRequest.AmountToCharge.Amount, paymentRequest.AmountToCharge.Currency)));
            return payment;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string paymentId)
        {
            var payment = await _paymentRepository.GetAsync(paymentId);
            return Ok(payment);
        }
        
    }

    public class PaymentResponseDto
    {
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentId { get; set; }
    }

    public class PaymentRequestDto
    {
        public CardDto Card { get; set; }
        public string  MerchantId { get; set; }
        public MoneyDto AmountToCharge { get; set; }
        public string MerchantReference { get; set; }
    }

    public class MoneyDto
    {
        public int Amount { get; set; }
        public Currency Currency { get; set; }
    }

    public class CardDto
    {
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpirationDate { get; set; }
    }
}