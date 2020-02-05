using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentChallenge.Domain.Cards;
using PaymentChallenge.Domain.Payments;
using PaymentChallenge.Domain.Values;
using PaymentChallenge.WebApi.Controllers.Dto;
using ValidationResult = FluentValidation.Results.ValidationResult;

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

        /// <summary>
        /// Make a payment
        /// </summary>
        /// <param name="paymentRequestDto">paymentRequest</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/payments
        ///         {
        ///             "Card": {
        ///                 "CardNumber": "4242424242424242",
        ///                 "Cvv": "100",
        ///                 "ExpirationDate": "1212"
        ///             },
        ///             "MerchantId": "FancyShop",
        ///             "AmountToCharge": {
        ///                 "Amount": 1000,
        ///                 "Currency": 0
        ///             },
        ///             "MerchantReference": "eazeazea4d5q4s"
        ///         }
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(PaymentResponseDto))]
        [ProducesResponseType(400, Type = typeof(ValidationErrorDto))]
        public async Task<IActionResult> Post([FromBody][Required] PaymentRequestDto paymentRequestDto)
        {

            var command = CreateCommand(paymentRequestDto);
            var payment = await _paymentGateway.ProcessAsync(command);

            return payment.Match<IActionResult>(response => Created(@"/api/payments",new PaymentResponseDto
                {
                    PaymentId = response.PaymentId,
                    PaymentStatus = response.PaymentStatus
                }),
                result => new JsonResult(Detail(result))
                {
                    StatusCode = 400
                });
        }

        private static ValidationErrorDto Detail(ValidationResult result)
        {
            return new ValidationErrorDto()
            {
                Errors = result.Errors.Select(x => new Error
                {
                    Field = x.PropertyName,
                    Message = x.ErrorMessage
                }).ToList()
            };
        }

        public class Error
        {
            public string Field { get; set; }
            public string Message { get; set; }
        }
        public class ValidationErrorDto
        {
            public List<Error> Errors { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> Get(string paymentId)
        {
            var payment = await _paymentRepository.GetAsync(paymentId);
            return Ok(payment);
        }

        private static PaymentRequest CreateCommand(PaymentRequestDto paymentRequest)
        {
            return new PaymentRequest(
                DtoToModel(paymentRequest),
                paymentRequest.MerchantId, 
                DtoToModel(paymentRequest.AmountToCharge), 
                paymentRequest.MerchantReference);
        }

        private static Money DtoToModel(MoneyDto moneyDto)
        {
            return new Money(moneyDto.Amount, moneyDto.Currency);
        }

        private static Card DtoToModel(PaymentRequestDto paymentRequest)
        {
            return new Card(paymentRequest.Card.CardNumber, paymentRequest.Card.Cvv, paymentRequest.Card.ExpirationDate);
        }
    }
}