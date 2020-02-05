using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        public PaymentsController(ILogger<PaymentsController> logger, PaymentGateway paymentGateway)
        {
            _logger = logger;
            _paymentGateway = paymentGateway;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PaymentRequestDto paymentRequest)
        {
            var card = new Card(paymentRequest.Card.CardNumber, paymentRequest.Card.Cvv, paymentRequest.Card.ExpirationDate );
            var payment = await _paymentGateway.ProcessAsync(new PaymentRequest(card, paymentRequest.MerchantId,
                new Money(paymentRequest.AmountToCharge.Amount, paymentRequest.AmountToCharge.Currency)));
            return payment.Match(response => Ok(response),
                result => Problem(result.ToString()));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
        
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